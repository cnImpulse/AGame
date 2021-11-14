using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;

namespace SSRPG
{
    public class AutoActionState : BattleUnitBaseState
    {
        protected override void OnEnter(IFsm<BattleUnit> fsm)
        {
            base.OnEnter(fsm);

            var skillCfg = GameEntry.Cfg.Tables.TblSkill.Get(SelectSkill());
            BattleUnit attackTarget = FindTarget(skillCfg);
            if (attackTarget == null)
            {
                GameEntry.Fsm.StartCoroutine(AutoAction(Owner));
                return;
            }

            var path = new List<GridData>();
            var end = FindMoveEnd(attackTarget, skillCfg);
            bool result = GameEntry.Navigator.Navigate(m_GridMap.Data, Owner, end, out path);
            if (result == true)
            {
                GameEntry.Fsm.StartCoroutine(AutoAction(Owner, attackTarget, path));
            }
            else
            {
                GameEntry.Fsm.StartCoroutine(AutoAction(Owner));
            }
        }

        protected override void OnUpdate(IFsm<BattleUnit> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<BattleUnit> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

        #region 战斗单位自动行动AI

        // 1. 获取移动前的所有可攻击单元格
        // 2. 找到范围内的第一个可攻击单位
        // 3. 根据攻击范围寻找移动终点, 远程单位会尽量让敌人离自身最远
        // 4. 移动到目标点, 攻击

        private int SelectSkill()
        {
            return Owner.Data.SkillList[0];
        }

        /// <summary>
        /// 寻找攻击目标
        /// </summary>
        private BattleUnit FindTarget(Cfg.Battle.Skill skillCfg)
        {
            var canAttackList = m_GridMap.Data.GetCanAttackGrids(Owner, skillCfg.ReleaseRange, true);

            var targetCamp = skillCfg.SkillType == Cfg.Battle.SkillType.Damage ?
                BattleUtl.GetHostileCamp(Owner.Data.CampType) : Owner.Data.CampType;
            foreach (var gridData in canAttackList)
            {
                GridUnit gridUnit = gridData.GridUnit;
                if (gridUnit != null && gridUnit is BattleUnit &&
                    gridUnit.Data.CampType == targetCamp)
                {
                    return gridUnit as BattleUnit;
                }
            }
            return null;
        }

        /// <summary>
        /// 寻找移动终点
        /// </summary>
        private GridData FindMoveEnd(BattleUnit attackTarget, Cfg.Battle.Skill skillCfg)
        {
            if (attackTarget == null)
            {
                return null;
            }

            GridData end = null;
            var canMoveList = m_GridMap.Data.GetCanMoveGrids(Owner);
            foreach (var gridData in canMoveList)
            {
                int distance = GridMapUtl.GetDistance(attackTarget.GridData, gridData);
                int atkRange = skillCfg.ReleaseRange;
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

        public IEnumerator ShowMoveArea(BattleUnit battleUnit)
        {
            int effectId = GameEntry.Effect.CreatEffect(Cfg.Effect.EffectType.Select, battleUnit.transform.position);
            yield return new WaitForSeconds(1.5f);
            GameEntry.Effect.HideEffect(effectId);

            var canMoveList = m_GridMap.Data.GetCanMoveGrids(battleUnit);
            m_GridMap.ShowMoveArea(canMoveList);
            yield return new WaitForSeconds(0.8f);
            GameEntry.Effect.HideGridMapEffect();
        }

        public IEnumerator AutoAction(BattleUnit battleUnit)
        {
            yield return ShowMoveArea(battleUnit);

            ChangeState<EndActionState>();
        }

        public IEnumerator AutoAction(BattleUnit battleUnit, BattleUnit attackTarget, List<GridData> path)
        {
            yield return ShowMoveArea(battleUnit);

            foreach (var gridData in path)
            {
                battleUnit.transform.position = m_GridMap.GridPosToWorldPos(gridData.GridPos);
                yield return new WaitForSeconds(0.3f);
            }
            if (path.Count >= 1)
            {
                m_GridMap.MoveTo(battleUnit, path[path.Count - 1].GridPos);
            }

            var skillId = SelectSkill();
            var canReleaseList = m_GridMap.Data.GetSkillReleaseRange(battleUnit, skillId);
            m_GridMap.ShowAttackArea(canReleaseList);
            yield return new WaitForSeconds(0.5f);
            GameEntry.Effect.HideGridMapEffect();

            yield return new WaitForSeconds(0.2f);
            GameEntry.Skill.RequestReleaseSkill(skillId, battleUnit.Id, attackTarget.Id);

            ChangeState<EndActionState>();
        }
    }
}