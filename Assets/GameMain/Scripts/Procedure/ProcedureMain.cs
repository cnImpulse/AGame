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

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("游戏主流程。");

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenMainFormSuccess);

            GameEntry.UI.OpenUIForm(UIFormId.MainForm);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (m_MainForm != null)
            {
                m_MainForm.Close(isShutdown);
            }

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenMainFormSuccess);
            base.OnLeave(procedureOwner, isShutdown);
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
