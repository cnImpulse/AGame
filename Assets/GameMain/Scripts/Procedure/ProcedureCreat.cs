using GameFramework.Event;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace SSRPG
{
    public class ProcedureLoadSaveData : ProcedureBase
    {
        private RewardForm m_Form = null;
        private bool m_EndProcedure = false;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("加载存档流程。");

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenRewardForm);
            GameEntry.Event.Subscribe(EventName.EnsureReward, OnEnsureReward);

            GameEntry.Save.InitSaveData();
            m_EndProcedure = GameEntry.Save.SaveData.EndFirstGuide;
            if (!m_EndProcedure)
            {
                GameEntry.UI.OpenUIForm(Cfg.UI.FormType.StoryForm);
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_EndProcedure)
            {
                ChangeState<ProcedureMain>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            m_EndProcedure = false;
            if (m_Form != null)
            {
                m_Form.Close(isShutdown);
                m_Form = null;
            }

            GameEntry.GameTips.StopAllTips();
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenRewardForm);
            GameEntry.Event.Unsubscribe(EventName.EnsureReward, OnEnsureReward);

            base.OnLeave(procedureOwner, isShutdown);
        }

        private void OnOpenRewardForm(object sender, GameEventArgs e)
        {
            var ne = (OpenUIFormSuccessEventArgs)e;

            if (ne.UIForm.Logic is RewardForm)
            {
                m_Form = ne.UIForm.Logic as RewardForm;
            }
        }

        private void OnEnsureReward(object sender, GameEventArgs e)
        {
            var ne = e as GameEventBase;
            if (sender.Equals(m_Form))
            {
                GameEntry.Save.AddDisciple(ne.UserData as VarInt32);
                GameEntry.Save.SaveData.EndFirstGuide = true;
                GameEntry.Save.Save();
                m_EndProcedure = true;
            }
        }
    }
}
