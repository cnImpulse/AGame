using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class SkillState : BattleUnitBaseState
    {
        private int m_SerilId = 0;
        private int m_SkillId = 0;
        private List<GridData> m_CanReleaseList = null;

        protected override void OnEnter(IFsm<BattleUnit> fsm)
        {
            base.OnEnter(fsm);

            GameEntry.Event.Subscribe(EventName.PointerDownGridMap, OnPointGridMap);

            m_SerilId = (int)GameEntry.UI.OpenUIForm(Cfg.UI.FormType.SkillActionForm, this);
        }

        protected override void OnUpdate(IFsm<BattleUnit> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<BattleUnit> fsm, bool isShutdown)
        {
            GameEntry.UI.CloseUIForm(isShutdown, m_SerilId);
            m_SerilId = 0;

            GameEntry.Effect.HideGridMapEffect();
            GameEntry.Event.Unsubscribe(EventName.PointerDownGridMap, OnPointGridMap);

            base.OnLeave(fsm, isShutdown);
        }

        public void ReleaseSkill(int skillId)
        {
            GameEntry.UI.CloseUIForm(m_SerilId);
            m_SkillId = skillId;
            m_CanReleaseList = m_GridMap.Data.GetSkillReleaseRange(Owner, m_SkillId);
            m_GridMap.ShowAttackArea(m_CanReleaseList);
        }

        private void OnPointGridMap(object sender, GameEventArgs e)
        {
            var ne = e as GameEventBase;
            var gridData = ne.UserData as GridData;
            var gridUnit = gridData.GridUnit;
            if (m_CanReleaseList.Contains(gridData))
            {
                if (gridUnit == null || !(gridUnit is BattleUnit))
                {
                    return;
                }

                if (GameEntry.Skill.RequestReleaseSkill(m_SkillId, Owner.Id, gridUnit.Id))
                {
                    ChangeState<EndActionState>();
                }
            }
            else
            {
                ChangeState<SkillState>();
            }
        }
    }
}