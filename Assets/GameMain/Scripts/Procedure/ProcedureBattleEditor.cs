using UnityEngine;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System.Collections.Generic;

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
        private bool m_Return = false;

        private GridMap m_GridMap = null;
        private BattleData m_BattleData = null;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("战斗编辑器。");

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowGirdMapSuccess);
            GameEntry.Event.Subscribe(PointerDownGridMapEventArgs.EventId, OnPointerDownGridMap);

            GameEntry.UI.OpenUIForm(UIFormId.BattleEditorForm, this);
            InitBattleEditor();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_Return)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", (int)SceneType.Menu);
                ChangeState<ProcedureChangeScene>(procedureOwner);
                return;
            }

            // 后面绘制逻辑改为放到状态机里面
            if (m_GridMap == null || m_Form.EditMode != EditMode.Fill)
            {
                return;
            }

            var gridPos = m_GridMap.WorldPosToGridPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (Input.GetMouseButtonDown(0))
            {
                OnBeginDragGridMap(gridPos);
            }
            else if (Input.GetMouseButton(0))
            {
                OnDragGridMap(gridPos);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnDropGridMap(gridPos);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            SaveBattleData();

            m_Return = false;
            m_GridMap = null;
            m_BattleData = null;
            if (m_Form != null)
            {
                m_Form.Close(isShutdown);
                m_Form = null;
            }

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowGirdMapSuccess);
            GameEntry.Event.Unsubscribe(PointerDownGridMapEventArgs.EventId, OnPointerDownGridMap);

            base.OnLeave(procedureOwner, isShutdown);
        }

        public void ReturnMenuForm()
        {
            m_Return = true;
        }

        private void InitBattleEditor()
        {
            GridMapData gridMapData = new GridMapData(18, 10, 1);
            GameEntry.Entity.ShowGridMap(gridMapData);
            m_BattleData = new BattleData(gridMapData);
        }

        private void SaveBattleData()
        {
            var enemyList = m_GridMap.GetBattleUnitList(CampType.Enemy);
            foreach (var enemy in enemyList)
            {
                m_BattleData.enemyList.Add(m_GridMap.Data.GridPosToIndex(enemy.Data.GridPos), enemy.Data.TypeId);
            }

            var gridList = new List<GridData>();
            foreach (var gridData in m_BattleData.gridMapData.GridList.Values)
            {
                if (gridData.CanArrive())
                {
                    gridList.Add(gridData);
                }
            }

            m_BattleData.maxPlayerBattleUnit = enemyList.Count;
            for (int i=0; i<m_BattleData.maxPlayerBattleUnit; ++i)
            {
                int index = Random.Range(0, gridList.Count - 1);
                m_BattleData.playerBrithList.Add(gridList[index].GridPos);
            }

            string path = AssetUtl.GetBattleDataPath(1);
            AssetUtl.SaveData(path, m_BattleData);
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
                var data = new BattleUnitData(m_Form.SelectedBattleUnitId, m_GridMap.Id, ne.gridData.GridPos, CampType.Enemy);
                m_GridMap.RegisterBattleUnit(data);
            }
            else if (m_Form.EditMode == EditMode.Erase)
            {
                if (ne.gridData.GridUnit != null)
                {
                    m_GridMap.UnRegisterGridUnit(ne.gridData.GridUnit);
                }
                else
                {
                    m_GridMap.SetGridData(ne.gridData.GridPos, GridType.Normal);
                }
            }
        }

        private Vector2Int dragStart = default;
        private void OnBeginDragGridMap(Vector2Int gridPos)
        {
            dragStart = gridPos;
        }

        private Vector2Int lastPos = default;
        private void OnDragGridMap(Vector2Int gridPos)
        {
            if (lastPos == gridPos)
            {
                return;
            }

            lastPos = gridPos;
            var gridList = m_GridMap.Data.GetBoxGridList(dragStart, gridPos);
            m_GridMap.RefreshPreview(gridList, GridType.Wall);
        }

        private void OnDropGridMap(Vector2Int gridPos)
        {
            var gridList = m_GridMap.Data.GetBoxGridList(dragStart, gridPos);
            foreach (var grid in gridList)
            {
                grid.GridType = GridType.Wall;
            }

            m_GridMap.RefreshMap();
            dragStart = default;
        }
    }
}
