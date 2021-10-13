using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 玩家回合，战斗单位移动状态
    /// </summary>
    public class BattleUnitMoveState : FsmState<ProcedureBattle>
    {
        private GridMap m_GridMap = null;
        private BattleUnit m_ActiveBattleUnit = null;
        private List<GridData> m_CanMoveList = null;
        private bool m_EndMove = false;

        protected override void OnEnter(IFsm<ProcedureBattle> fsm)
        {
            base.OnEnter(fsm);

            Log.Info("进入移动状态。");

            GameEntry.Event.Subscribe(PointGridMapEventArgs.EventId, OnPointGridMap);

            m_GridMap = GameEntry.Battle.GridMap;
            m_ActiveBattleUnit = GameEntry.Battle.ActiveBattleUnit;
            m_CanMoveList = m_GridMap.Data.GetCanMoveGrids(m_ActiveBattleUnit);
            m_GridMap.ShowMoveArea(m_CanMoveList);
        }

        protected override void OnUpdate(IFsm<ProcedureBattle> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            if (m_ActiveBattleUnit == null)
            {
                if (m_EndMove)
                {
                    ChangeState<PlayerActionState>(fsm);
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

            m_EndMove = false;
            m_ActiveBattleUnit = null;
            m_GridMap.HideTilemapEffect();

            GameEntry.Event.Unsubscribe(PointGridMapEventArgs.EventId, OnPointGridMap);

            Log.Info("离开移动状态。");
        }

        private void OnPointGridMap(object sender, GameEventArgs e)
        {
            PointGridMapEventArgs ne = (PointGridMapEventArgs)e;
            GridUnit gridUnit = ne.gridData.GridUnit;
            if (m_CanMoveList.Contains(ne.gridData))
            {
                m_ActiveBattleUnit.Move(ne.gridData.GridPos);
                m_EndMove = true;

                Log.Info("移动到：{0}", ne.gridData.GridPos);
            }
            else if (gridUnit != null && gridUnit.Data.GridUnitType == GridUnitType.BattleUnit)
            {
                if (gridUnit == m_ActiveBattleUnit)
                {
                    m_EndMove = true;
                }
                else
                {
                    GameEntry.Battle.SelectBattleUnit = gridUnit as BattleUnit;
                }
            }
            m_ActiveBattleUnit = null;
        }
    }
}