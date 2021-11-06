using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class SkillState : BattleUnitBaseState
    {
        private int m_SerilId = 0;

        protected override void OnEnter(IFsm<BattleUnit> fsm)
        {
            base.OnEnter(fsm);

            m_SerilId = (int)GameEntry.UI.OpenUIForm(Cfg.UI.FormType.SkillActionForm, this);
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
    }
}