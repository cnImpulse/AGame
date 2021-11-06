using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class AttackState : BattleUnitBaseState
    {
        private List<GridData> m_CanAttackList = null;

        protected override void OnEnter(IFsm<BattleUnit> fsm)
        {
            base.OnEnter(fsm);

            GameEntry.Event.Subscribe(EventName.PointerDownGridMap, OnPointGridMap);

            m_CanAttackList = m_GridMap.Data.GetCanAttackGrids(Owner);
            m_GridMap.ShowAttackArea(m_CanAttackList);
        }

        protected override void OnUpdate(IFsm<BattleUnit> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<BattleUnit> fsm, bool isShutdown)
        {
            GameEntry.Effect.HideGridMapEffect();
            GameEntry.Event.Unsubscribe(EventName.PointerDownGridMap, OnPointGridMap);

            base.OnLeave(fsm, isShutdown);
        }

        private void OnPointGridMap(object sender, GameEventArgs e)
        {
            var ne = e as GameEventBase;
            var gridData = ne.UserData as GridData;
            if (m_CanAttackList.Contains(gridData))
            {
                if (gridData.GridUnit == null)
                {
                    return;
                }

                Owner.Attack(gridData);
                ChangeState<EndActionState>();
            }
            else
            {
                ChangeState<ActionState>();
            }
        }
    }
}