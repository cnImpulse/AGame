using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 技能释放状态
    /// </summary>
    public class SkillReleaseState : FsmState<ProcedureBattle>
    {
        private GridMap m_GridMap = null;
        private bool m_EndAction = false;
        private int m_SkillId = 0;
        private BattleUnit m_ActiveBattleUnit = null;
        private List<GridData> m_CanReleaseList = null;

        protected override void OnEnter(IFsm<ProcedureBattle> fsm)
        {
            base.OnEnter(fsm);

            Log.Info("进入技能释放状态。");

            GameEntry.Event.Subscribe(PointGridMapEventArgs.EventId, OnPointGridMap);

            m_GridMap = GameEntry.Battle.GridMap;
            m_SkillId = GameEntry.Battle.ActionArg;
            m_ActiveBattleUnit = GameEntry.Battle.ActiveBattleUnit;
            m_CanReleaseList = m_GridMap.Data.GetSkillReleaseRange(m_ActiveBattleUnit, m_SkillId);
            m_GridMap.ShowAttackArea(m_CanReleaseList);
        }

        protected override void OnUpdate(IFsm<ProcedureBattle> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            if (m_ActiveBattleUnit == null)
            {
                if (m_EndAction)
                {
                    ChangeState<BattleUnitEndActionState>(fsm);
                }
                else
                {
                    ChangeState<PlayerActionState>(fsm);
                }
            }
        }

        protected override void OnLeave(IFsm<ProcedureBattle> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            m_EndAction = false;
            m_CanReleaseList = null;
            m_ActiveBattleUnit = null;
            m_GridMap.HideTilemapEffect();

            GameEntry.Event.Unsubscribe(PointGridMapEventArgs.EventId, OnPointGridMap);
        }

        private void OnPointGridMap(object sender, GameEventArgs e)
        {
            PointGridMapEventArgs ne = (PointGridMapEventArgs)e;
            GridUnit gridUnit = ne.gridData.GridUnit;
            if (m_CanReleaseList.Contains(ne.gridData)) 
            {
                if (gridUnit == null || gridUnit.Data.GridUnitType != GridUnitType.BattleUnit)
                {
                    return;
                }

                if (GameEntry.Skill.RequestReleaseSkill(m_SkillId, m_ActiveBattleUnit.Id, ne.gridData.GridUnit.Id))
                {
                    m_EndAction = true;
                }
            }
            m_ActiveBattleUnit = null;
        }
    }
}