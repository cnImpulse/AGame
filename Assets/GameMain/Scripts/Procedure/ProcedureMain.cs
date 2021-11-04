using System.Collections.Generic;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace SSRPG
{
    public class ProcedureMain : ProcedureBase
    {
        private MainForm m_MainForm = null;

        private bool m_Explore = false;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("游戏主流程。");

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenMainFormSuccess);

            GameEntry.UI.OpenUIForm(UIFormId.MainForm, this);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_Explore)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", (int)SceneType.Battle);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            m_Explore = false;
            if (m_MainForm != null)
            {
                m_MainForm.Close(isShutdown);
            }

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenMainFormSuccess);
            base.OnLeave(procedureOwner, isShutdown);
        }

        public void EnterExplore()
        {
            m_Explore = true;
        }

        private void OnOpenMainFormSuccess(object sender, GameEventArgs e)
        {
            var ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UIForm.Logic is MainForm)
            {
                m_MainForm = ne.UIForm.Logic as MainForm;
            }
        }
    }
}
