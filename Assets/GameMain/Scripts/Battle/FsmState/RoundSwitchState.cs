using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 玩家回合，选择战斗单位状态
    /// </summary>
    public class RoundSwitchState : FsmState<ProcedureBattle>
    {
        private CampType m_ActiveCamp = CampType.None;

        protected override void OnEnter(IFsm<ProcedureBattle> fsm)
        {
            base.OnEnter(fsm);

            Log.Info("进入回合切换状态。");

            CampType endCampType = m_ActiveCamp;
            if (m_ActiveCamp == CampType.None || m_ActiveCamp == CampType.Enemy)
            {
                m_ActiveCamp = CampType.Player;
            }
            else if (m_ActiveCamp == CampType.Player)
            {
                m_ActiveCamp = CampType.Enemy;
            }

            GameEntry.Event.Fire(this, RoundSwitchEventArgs.Create(endCampType, m_ActiveCamp));
            Log.Info(m_ActiveCamp);
        }

        protected override void OnUpdate(IFsm<ProcedureBattle> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            if (GameEntry.Battle.AutoBattle || m_ActiveCamp != CampType.Player)
            {
                ChangeState<AutoActionState>(fsm);
            }
            else
            {
                ChangeState<SelectBattleUnitState>(fsm);
            }
        }

        protected override void OnLeave(IFsm<ProcedureBattle> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            fsm.Owner.activeCamp = m_ActiveCamp;
        }
    }
}