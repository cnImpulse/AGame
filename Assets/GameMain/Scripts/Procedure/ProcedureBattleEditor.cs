using GameFramework.Event;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace SSRPG
{
    public enum EditMode
    {
        None,
        Paint,
        Fill,
        Erase,
    }

    public class ProcedureBattleEditor : ProcedureBase
    {
        private BattleEditorForm m_Form = null;

        private GridMap m_GridMap = null;
        private GridMapData m_EditData => m_GridMap.Data;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("战斗编辑器。");

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowGirdMapSuccess);
            GameEntry.Event.Subscribe(PointerDownGridMapEventArgs.EventId, OnPointerDownGridMap);
            GameEntry.Event.Subscribe(PointerDragBeginGridMapEventArgs.EventId, OnBeginDragGridMap);
            GameEntry.Event.Subscribe(PointerDragGridMapEventArgs.EventId, OnDragGridMap);
            GameEntry.Event.Subscribe(PointerDropGridMapEventArgs.EventId, OnDropGridMap);

            GameEntry.UI.OpenUIForm(UIFormId.BattleEditorForm, this);
            CreatGridMap();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (m_Form != null)
            {
                m_Form.Close(isShutdown);
                m_Form = null;
            }

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowGirdMapSuccess);
            GameEntry.Event.Unsubscribe(PointerDownGridMapEventArgs.EventId, OnPointerDownGridMap);
            GameEntry.Event.Unsubscribe(PointerDragBeginGridMapEventArgs.EventId, OnBeginDragGridMap);
            GameEntry.Event.Unsubscribe(PointerDragGridMapEventArgs.EventId, OnDragGridMap);
            GameEntry.Event.Unsubscribe(PointerDropGridMapEventArgs.EventId, OnDropGridMap);

            base.OnLeave(procedureOwner, isShutdown);
        }

        private void CreatGridMap()
        {
            GridMapData gridMapData = new GridMapData(1);
            GameEntry.Entity.ShowGridMap(gridMapData);
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_Form = (BattleEditorForm)ne.UIForm.Logic;
        }

        private void OnShowGirdMapSuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType == typeof(GridMap))
            {
                m_GridMap = ne.Entity.Logic as GridMap;
            }
        }

        private void OnPointerDownGridMap(object sender, GameEventArgs e)
        {
            PointerDownGridMapEventArgs ne = (PointerDownGridMapEventArgs)e;

            if (!sender.Equals(m_GridMap))
            {
                Log.Warning("点击的地图不可编辑!");
            }

            if (m_Form.EditMode == EditMode.None)
            {
                return;
            }

            if (m_Form.EditMode == EditMode.Paint)
            {
                m_GridMap.SetGridData(ne.gridData.GridPos, GridType.Wall);
            }
            else if (m_Form.EditMode == EditMode.Erase)
            {
                m_GridMap.SetGridData(ne.gridData.GridPos, GridType.Normal);
            }
        }

        private GridData dragStart = null;

        private void OnBeginDragGridMap(object sender, GameEventArgs e)
        {
            var ne = (PointerDragBeginGridMapEventArgs)e;

            if (m_Form.EditMode != EditMode.Fill)
            {
                return;
            }

            dragStart = ne.gridData;
        }

        private void OnDragGridMap(object sender, GameEventArgs e)
        {
            var ne = (PointerDragGridMapEventArgs)e;

            var dragEnd = ne.gridData;
            var gridList = m_GridMap.Data.GetBoxGridList(dragStart.GridPos, dragEnd.GridPos);
            m_GridMap.RefreshPreview(gridList, GridType.Wall);
        }

        private void OnDropGridMap(object sender, GameEventArgs e)
        {
            var ne = (PointerDropGridMapEventArgs)e;

            var dragEnd = ne.gridData;
            var gridList = m_GridMap.Data.GetBoxGridList(dragStart.GridPos, dragEnd.GridPos);
            foreach (var grid in gridList)
            {
                grid.GridType = GridType.Wall;
            }

            m_GridMap.RefreshMap();
            dragStart = null;
        }
    }
}
