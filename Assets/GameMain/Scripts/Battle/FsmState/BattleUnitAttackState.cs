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
    public class BattleUnitAttackState : FsmState<ProcedureBattle>
    {
        private GridMap m_GridMap = null;

        private bool m_EndAttack = false;
        private BattleUnit m_ActiveBattleUnit = null;
        private List<GridData> m_CanAttackList = null;

        protected override void OnEnter(IFsm<ProcedureBattle> fsm)
        {
            base.OnEnter(fsm);

            Log.Info("进入攻击状态。");

            GameEntry.Event.Subscribe(PointGridMapEventArgs.EventId, OnPointGridMap);

            if (m_GridMap == null)
            {
                m_GridMap = fsm.Owner.gridMap;
            }

            m_ActiveBattleUnit = GameEntry.Battle.ActiveBattleUnit;
            m_CanAttackList = m_GridMap.Data.GetCanAttackGrids(m_ActiveBattleUnit);
            m_GridMap.ShowAttackArea(m_CanAttackList);
        }

        protected override void OnUpdate(IFsm<ProcedureBattle> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            if (m_ActiveBattleUnit == null)
            {
                if (m_EndAttack)
                {
                    ChangeState<BattleUnitEndActionState>(fsm);
                }
                else
                {
                    ChangeState<BattleUnitActionState>(fsm);
                }
            }
        }

        protected override void OnLeave(IFsm<ProcedureBattle> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            m_EndAttack = false;
            m_CanAttackList = null;
            m_ActiveBattleUnit = null;
            m_GridMap.HideTilemapEffect();

            GameEntry.Event.Unsubscribe(PointGridMapEventArgs.EventId, OnPointGridMap);

            Log.Info("离开攻击状态。");
        }

        private void OnPointGridMap(object sender, GameEventArgs e)
        {
            PointGridMapEventArgs ne = (PointGridMapEventArgs)e;
            if (m_CanAttackList.Contains(ne.gridData)) 
            {
                if (ne.gridData.GridUnit == null)
                {
                    return;
                }

                Log.Info("攻击：{0}", ne.gridData.GridPos);

                m_ActiveBattleUnit.Attack(ne.gridData);
                m_EndAttack = true;
            }
            m_ActiveBattleUnit = null;
        }
    }
}