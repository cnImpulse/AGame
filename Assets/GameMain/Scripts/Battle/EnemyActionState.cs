using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 敌方回合，进入敌方行动状态
    /// </summary>
    public class EnemyActionState : FsmState<ProcedureBattle>
    {
        private GridMap m_GridMap = null;

        private bool m_EndAction = false;
        private BattleUnit m_ActiveBattleUnit = null;

        protected override void OnEnter(IFsm<ProcedureBattle> fsm)
        {
            base.OnEnter(fsm);

            Log.Info("进入敌方行动状态。");

            if (m_GridMap == null)
            {
                m_GridMap = fsm.Owner.gridMap;
            }

            foreach (var battleUnit in fsm.Owner.enemyBattleUnits)
            {
                if (battleUnit.CanAction)
                {
                    m_ActiveBattleUnit = battleUnit;
                }
            }

            VarObject data = new VarObject();
            data.Value = m_ActiveBattleUnit;
            fsm.SetData("ActiveBattleUnit", data);

            BattleUnit attackTarget = FindAttackTarget();
            if (attackTarget != null && m_ActiveBattleUnit != null)
            {
                List<GridData> path = new List<GridData>();

                GridData end = m_GridMap.GridMapData.GetGridData(attackTarget.GridUnitData.GridPos);
                GameEntry.Navigator.Navigate(m_GridMap.GridMapData, m_ActiveBattleUnit.BattleUnitData, end, out path);

                m_GridMap.StartCoroutine(EnemyActionAnim(m_ActiveBattleUnit, attackTarget, path));
            }
        }

        protected override void OnUpdate(IFsm<ProcedureBattle> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            if (m_EndAction && m_ActiveBattleUnit)
            {
                ChangeState<BattleUnitEndActionState>(fsm);
            }
        }

        protected override void OnLeave(IFsm<ProcedureBattle> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            m_EndAction = false;
            m_ActiveBattleUnit = null;
            Log.Info("离开敌方行动状态。");
        }

        #region 敌方战斗单位AI

        private BattleUnit FindAttackTarget()
        {
            var canAttackList = m_GridMap.GridMapData.GetCanAttackGrids(m_ActiveBattleUnit.BattleUnitData, true);

            foreach (var gridData in canAttackList)
            {
                GridUnit gridUnit = gridData.GridUnit;
                if (gridUnit != null && gridUnit.GridUnitData.GridUnitType == GridUnitType.BattleUnit &&
                    gridUnit.GridUnitData.CampType == CampType.Player)
                {
                    return gridUnit as BattleUnit;
                }
            }
            return null;
        }

        #endregion

        public IEnumerator EnemyActionAnim(BattleUnit battleUnit, BattleUnit attackTarget, List<GridData> path)
        {
            int effectId = GameEntry.Effect.CreatEffect(EffectType.SelectType, battleUnit.transform.position);
            yield return new WaitForSeconds(2f);
            GameEntry.Effect.DestoryEffect(effectId);

            var canMoveList = m_GridMap.GridMapData.GetCanMoveGrids(battleUnit.BattleUnitData);
            m_GridMap.ShowMoveArea(canMoveList);
            yield return new WaitForSeconds(0.8f);
            m_GridMap.HideTilemapEffect();

            foreach (var gridData in path)
            {
                battleUnit.Move(gridData.GridPos);
                yield return new WaitForSeconds(0.3f);
            }

            var canAttackList = m_GridMap.GridMapData.GetCanAttackGrids(battleUnit.BattleUnitData);
            m_GridMap.ShowAttackArea(canAttackList);
            yield return new WaitForSeconds(0.5f);
            m_GridMap.HideTilemapEffect();

            yield return new WaitForSeconds(0.2f);
            battleUnit.Attack(attackTarget.GridData);

            m_EndAction = true;
        }
    }
}