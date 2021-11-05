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

            m_SerilId = (int)GameEntry.UI.OpenUIForm(UIFormId.ActionForm, this);
        }

        protected override void OnUpdate(IFsm<BattleUnit> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<BattleUnit> fsm, bool isShutdown)
        {
            GameEntry.UI.CloseUIForm(m_SerilId, isShutdown);
            m_SerilId = 0;

            base.OnLeave(fsm, isShutdown);
        }

        public void SelectAction(ActionType actionType)
        {
            if (actionType == ActionType.Attack)
            {
                ChangeState<AttackState>();
            }
            else if (actionType == ActionType.Skill)
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