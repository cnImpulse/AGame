using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class ActionState : BattleUnitBaseState
    {
        private int m_SerilId = 0;

        protected override void OnEnter(IFsm<BattleUnit> fsm)
        {
            base.OnEnter(fsm);

            m_SerilId = GameEntry.UI.OpenUIForm(Cfg.UI.FormType.ActionForm, this);
        }

        protected override void OnUpdate(IFsm<BattleUnit> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<BattleUnit> fsm, bool isShutdown)
        {
            GameEntry.UI.CloseUIForm(isShutdown, m_SerilId);
            m_SerilId = 0;

            base.OnLeave(fsm, isShutdown);
        }

        public void SelectAction(ActionType actionType)
        {
            if (actionType == ActionType.Skill)
            {
                ChangeState<SkillState>();
            }
            else if (actionType == ActionType.Await)
            {
                ChangeState<EndActionState>();
            }
        }
    }
}