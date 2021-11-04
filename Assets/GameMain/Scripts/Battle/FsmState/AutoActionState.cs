using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 自动行动状态
    /// </summary>
    public class AutoActionState : FsmState<ProcedureBattle>
    {
        private GridMap m_GridMap = null;

        private bool m_EndAction = false;
        private BattleUnit m_ActiveBattleUnit = null;

        protected override void OnEnter(IFsm<ProcedureBattle> fsm)
        {
            base.OnEnter(fsm);

            Log.Info("进入自动行动状态。");

            m_GridMap = GameEntry.Battle.GridMap;
            foreach (var battleUnit in m_GridMap.GetBattleUnitList(GameEntry.Battle.ActiveCampType))
            {
                if (battleUnit.CanAction)
                {
                    m_ActiveBattleUnit = battleUnit;
                }
            }

            GameEntry.Battle.ActiveBattleUnit = m_ActiveBattleUnit;
            BattleUnit attackTarget = FindAttackTarget();
            if (attackTarget != null && m_ActiveBattleUnit != null)
            {
                List<GridData> path = new List<GridData>();

                GridData end = FindMoveEnd(attackTarget);
                bool result = GameEntry.Navigator.Navigate(m_GridMap.Data, m_ActiveBattleUnit, end, out path);
                if (result == true)
                {
                    GameEntry.Fsm.StartCoroutine(BattleUnitAutoAction(m_ActiveBattleUnit, attackTarget, path));
                }
                else
                {
                    m_EndAction = true;
                }
            }
            else
            {
                m_EndAction = true;
            }
        }

        protected override void OnUpdate(IFsm<ProcedureBattle> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            if (m_EndAction)
            {
                ChangeState<BattleUnitEndActionState>(fsm);
            }
        }

        protected override void OnLeave(IFsm<ProcedureBattle> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            m_EndAction = false;
            m_ActiveBattleUnit = null;
        }

        #region 战斗单位自动行动AI

        // 1. 获取移动前的所有可攻击单元格
        // 2. 找到范围内的第一个可攻击单位
        // 3. 根据攻击范围寻找移动终点, 远程单位会尽量让敌人离自身最远
        // 4. 移动到目标点, 攻击

        /// <summary>
        /// 寻找攻击目标
        /// </summary>
        private BattleUnit FindAttackTarget()
        {
            var canAttackList = m_GridMap.Data.GetCanAttackGrids(m_ActiveBattleUnit, true);

            foreach (var gridData in canAttackList)
            {
                GridUnit gridUnit = gridData.GridUnit;
                if (gridUnit != null && gridUnit.Data.GridUnitType == GridUnitType.BattleUnit &&
                    gridUnit.Data.CampType != m_ActiveBattleUnit.Data.CampType)
                {
                    return gridUnit as BattleUnit;
                }
            }
            return null;
        }

        /// <summary>
        /// 寻找移动终点
        /// </summary>
        private GridData FindMoveEnd(BattleUnit attackTarget)
        {
            if (attackTarget == null)
            {
                return null;
            }

            GridData end = null;
            var canMoveList = m_GridMap.Data.GetCanMoveGrids(m_ActiveBattleUnit);
            foreach (var gridData in canMoveList)
            {
                int distance = GridMapUtl.GetDistance(attackTarget.GridData, gridData);
                int atkRange = m_ActiveBattleUnit.Data.AtkRange;
                if (distance > atkRange)
                {
                    continue;
                }

                if (end == null)
                {
                    end = gridData;
                    continue;
                }

                int tempDis = GridMapUtl.GetDistance(attackTarget.GridData, end);
                if (tempDis < distance)
                    //(tempDis == distance && GridMapUtl.GetNearestGridData(m_ActiveBattleUnit.GridData, end, gridData) == gridData))
                {
                    end = gridData;
                }
            }

            return end;
        }

        #endregion

        public IEnumerator BattleUnitAutoAction(BattleUnit battleUnit, BattleUnit attackTarget, List<GridData> path)
        {
            int effectId = GameEntry.Effect.CreatEffect(EffectId.SelectGridUnit, battleUnit.transform.position);
            yield return new WaitForSeconds(1.5f);
            GameEntry.Effect.DestoryEffect(effectId);

            var canMoveList = m_GridMap.Data.GetCanMoveGrids(battleUnit);
            m_GridMap.ShowMoveArea(canMoveList);
            yield return new WaitForSeconds(0.8f);
            GameEntry.Effect.HideGridMapEffect();

            foreach (var gridData in path)
            {
                battleUnit.transform.position = m_GridMap.GridPosToWorldPos(gridData.GridPos);
                yield return new WaitForSeconds(0.3f);
            }
            if (path.Count >= 1)
            {
                m_GridMap.MoveTo(battleUnit, path[path.Count - 1].GridPos);
            }

            var canAttackList = m_GridMap.Data.GetCanAttackGrids(battleUnit);
            m_GridMap.ShowAttackArea(canAttackList);
            yield return new WaitForSeconds(0.5f);
            GameEntry.Effect.HideGridMapEffect();

            yield return new WaitForSeconds(0.2f);
            battleUnit.Attack(attackTarget.GridData);

            m_EndAction = true;
        }
    }
}