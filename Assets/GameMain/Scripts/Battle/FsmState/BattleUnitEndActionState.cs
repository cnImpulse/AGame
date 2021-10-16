using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 战斗单位结束行动状态
    /// </summary>
    public class BattleUnitEndActionState : FsmState<ProcedureBattle>
    {
        private BattleUnit m_ActiveBattleUnit = null;

        protected override void OnEnter(IFsm<ProcedureBattle> fsm)
        {
            base.OnEnter(fsm);

            Log.Info("进入行动结束状态");

            m_ActiveBattleUnit = GameEntry.Battle.ActiveBattleUnit;
            m_ActiveBattleUnit.EndAction();
        }

        protected override void OnUpdate(IFsm<ProcedureBattle> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            if (GameEntry.Battle.NeedRoundSwitch)
            {
                ChangeState<RoundSwitchState>(fsm);
            }
            else
            {
                if (GameEntry.Battle.AutoBattle || m_ActiveBattleUnit.Data.CampType != CampType.Player)
                {
                    ChangeState<AutoActionState>(fsm);
                }
                else
                {
                    ChangeState<PlayerSelectState>(fsm);
                }
            }
        }

        protected override void OnLeave(IFsm<ProcedureBattle> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            m_ActiveBattleUnit = null;
            GameEntry.Battle.ActiveBattleUnit = null;
        }
    }
}