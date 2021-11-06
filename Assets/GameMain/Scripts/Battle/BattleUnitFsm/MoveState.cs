using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class MoveState : BattleUnitBaseState
    {
        private List<GridData> m_CanMoveList = null;

        protected override void OnEnter(IFsm<BattleUnit> fsm)
        {
            base.OnEnter(fsm);

            GameEntry.Event.Subscribe(EventName.PointerDownGridMap, OnPointGridMap);

            if (Owner.CanMove)
            {
                m_CanMoveList = m_GridMap.Data.GetCanMoveGrids(Owner);
                m_GridMap.ShowMoveArea(m_CanMoveList);
            }
            else
            {
                ChangeState<ActionState>(fsm);
            }
        }

        protected override void OnUpdate(IFsm<BattleUnit> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<BattleUnit> fsm, bool isShutdown)
        {
            m_CanMoveList = null;

            GameEntry.Effect.HideGridMapEffect();
            GameEntry.Event.Unsubscribe(EventName.PointerDownGridMap, OnPointGridMap);

            base.OnLeave(fsm, isShutdown);
        }

        private void OnPointGridMap(object sender, GameEventArgs e)
        {
            var ne = e as GameEventBase;
            var gridData = ne.UserData as GridData;
            var gridUnit = gridData.GridUnit;
            if (m_CanMoveList.Contains(gridData))
            {
                Owner.Move(gridData.GridPos);
                ChangeState<ActionState>();
            }
            else if (gridUnit == Owner)
            {
                ChangeState<ActionState>();
            }
            else
            {
                GameEntry.Event.Fire(this, EventName.BattleUnitActionCancel);
            }
        }
    }
}