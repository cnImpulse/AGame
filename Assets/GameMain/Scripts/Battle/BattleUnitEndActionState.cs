using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 玩家回合，战斗单位结束行动状态
    /// </summary>
    public class BattleUnitEndActionState : FsmState<ProcedureBattle>
    {
        private BattleUnit m_ActiveBattleUnit = null;

        protected override void OnEnter(IFsm<ProcedureBattle> fsm)
        {
            base.OnEnter(fsm);

            Log.Info("进入行动结束状态");

            m_ActiveBattleUnit = fsm.GetData("ActiveBattleUnit").GetValue() as BattleUnit;
            m_ActiveBattleUnit.EndAction();
        }

        protected override void OnUpdate(IFsm<ProcedureBattle> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            if (fsm.Owner.NeedRoundSwitch())
            {
                ChangeState<RoundSwitchState>(fsm);
            }
            else
            {
                if(m_ActiveBattleUnit.BattleUnitData.CampType == CampType.Player)
                {
                    ChangeState<SelectBattleUnitState>(fsm);
                }
                else if (m_ActiveBattleUnit.BattleUnitData.CampType == CampType.Enemy)
                {
                    ChangeState<EnemyActionState>(fsm);
                }
            }
        }

        protected override void OnLeave(IFsm<ProcedureBattle> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            m_ActiveBattleUnit = null;
            fsm.RemoveData("ActiveBattleUnit");

            Log.Info("离开行动结束状态");
        }
    }
}