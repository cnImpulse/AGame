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
            GameEntry.Event.Subscribe(EventName.PointerDownGridMap, OnPointGridMap);
        }

        protected override void OnUpdate(IFsm<ProcedureBattle> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<ProcedureBattle> fsm, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(EventName.BattleUnitActionEnd, OnBattleUnitActionEnd);
            GameEntry.Event.Unsubscribe(EventName.PointerDownGridMap, OnPointGridMap);

            base.OnLeave(fsm, isShutdown);
        }

        private void DestoryBattleUnitFsm()
        {
            if (m_BattleUnitFsm == null)
            {
                Log.Warning("战斗单位状态机为空.");
                return;
            }

            GameEntry.Fsm.DestroyFsm(m_BattleUnitFsm);
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
                    return;
                }
            }

            ChangeState<RoundEndState>();
        }

        private void OnBattleUnitActionCancel(object sender, GameEventArgs e)
        {
            DestoryBattleUnitFsm();
        }

        private void OnPointGridMap(object sender, GameEventArgs e)
        {
            var ne = e as GameEventBase;
            var gridData = ne.UserData as GridData;
            var gridUnit = gridData.GridUnit;
            if (gridUnit == null)
            {
                return;
            }

            var battleUnit = gridUnit as BattleUnit;
            if (m_BattleUnitFsm == null && battleUnit != null && battleUnit.CanAction)
            {
                m_BattleUnitFsm = GameEntry.Fsm.CreateFsm(battleUnit, new MoveState(),
                    new ActionState(), new AttackState(), new SkillState(), new EndActionState());
                m_BattleUnitFsm.Start<MoveState>();
            }
        }
    }
}