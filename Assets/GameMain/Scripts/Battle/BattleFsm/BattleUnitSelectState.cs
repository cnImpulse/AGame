using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class BattleUnitSelectState : BattleStateBase
    {
        private IFsm<BattleUnit> m_BattleUnitFsm = null;

        protected override void OnEnter(IFsm<ProcedureBattle> fsm)
        {
            base.OnEnter(fsm);

            GameEntry.Event.Subscribe(EventName.PointerDownGridMap, OnPointGridMap);
        }

        protected override void OnUpdate(IFsm<ProcedureBattle> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            if (IsAutoBattle)
            {
                var battleUnitList = m_GridMap.GetGridUnitList<BattleUnit>(m_ActiveCamp);
                foreach (var battleUnit in battleUnitList)
                {
                    if (m_BattleUnitFsm != null)
                    {
                        break;
                    }

                    CreatBattleUnitFsm(battleUnit);
                }
            }

            if (m_BattleUnitFsm != null)
            {
                ChangeState<BattleState>(fsm);
            }
        }

        protected override void OnLeave(IFsm<ProcedureBattle> fsm, bool isShutdown)
        {
            m_BattleUnitFsm = null;
            GameEntry.Event.Unsubscribe(EventName.PointerDownGridMap, OnPointGridMap);

            base.OnLeave(fsm, isShutdown);
        }

        private void CreatBattleUnitFsm(BattleUnit battleUnit)
        {
            if (!battleUnit.CanAction)
            {
                return;
            }

            if (IsAutoBattle)
            {
                m_BattleUnitFsm = GameEntry.Fsm.CreateFsm(battleUnit, new AutoActionState(), new EndActionState());
            }
            else
            {
                m_BattleUnitFsm = GameEntry.Fsm.CreateFsm(battleUnit, new MoveState(),
                    new ActionState(), new SkillState(), new EndActionState());
            }
            Fsm.SetData("BattleUnitFsm", new VarObject { Value = m_BattleUnitFsm });
        }

        private void OnPointGridMap(object sender, GameEventArgs e)
        {
            var ne = e as GameEventBase;
            var gridData = ne.UserData as GridData;
            GameEntry.Battle.ShowSelectEffect(m_GridMap.GridPosToWorldPos(gridData.GridPos));

            var gridUnit = gridData.GridUnit;
            if (gridUnit == null)
            {
                return;
            }

            CreatBattleUnitFsm(gridUnit as BattleUnit);
        }
    }
}