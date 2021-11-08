using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class BattleState : BattleStateBase
    {
        private IFsm<BattleUnit> m_BattleUnitFsm = null;

        protected override void OnEnter(IFsm<ProcedureBattle> fsm)
        {
            base.OnEnter(fsm);

            GameEntry.Event.Subscribe(EventName.BattleUnitActionCancel, OnBattleUnitActionCancel);
            GameEntry.Event.Subscribe(EventName.BattleUnitActionEnd, OnBattleUnitActionEnd);

            m_BattleUnitFsm = Fsm.GetData<VarObject>("BattleUnitFsm").Value as IFsm<BattleUnit>;
            if (IsAutoBattle)
            {
                m_BattleUnitFsm.Start<AutoActionState>();
            }
            else
            {
                m_BattleUnitFsm.Start<MoveState>();
            }
        }

        protected override void OnUpdate(IFsm<ProcedureBattle> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<ProcedureBattle> fsm, bool isShutdown)
        {
            DestoryBattleUnitFsm();
            GameEntry.Event.Unsubscribe(EventName.BattleUnitActionCancel, OnBattleUnitActionCancel);
            GameEntry.Event.Unsubscribe(EventName.BattleUnitActionEnd, OnBattleUnitActionEnd);

            base.OnLeave(fsm, isShutdown);
        }

        private void DestoryBattleUnitFsm()
        {
            if (m_BattleUnitFsm == null)
            {
                return;
            }

            GameEntry.Fsm.DestroyFsm(m_BattleUnitFsm);
            Fsm.RemoveData("BattleUnitFsm");
            m_BattleUnitFsm = null;
        }

        private void OnBattleUnitActionEnd(object sender, GameEventArgs e)
        {
            DestoryBattleUnitFsm();
            var battleUnitList = m_GridMap.GetGridUnitList<BattleUnit>(m_ActiveCamp);
            foreach (var battleUnit in battleUnitList)
            {
                if (battleUnit.CanAction)
                {
                    ChangeState<BattleUnitSelectState>();
                    return;
                }
            }
            
            ChangeState<RoundEndState>();
        }

        private void OnBattleUnitActionCancel(object sender, GameEventArgs e)
        {
            DestoryBattleUnitFsm();
            ChangeState<BattleUnitSelectState>();
        }
    }
}