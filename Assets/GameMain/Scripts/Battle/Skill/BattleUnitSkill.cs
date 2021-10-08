using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;

namespace SSRPG
{
    public class BattleUnitSkill : SkillBase
    {
        private BattleUnitData m_Caster = null;
        private BattleUnitData m_Target = null;

        private string m_Name = null;
        private float m_DamageRate = 0;
        private int m_ReleaseRange = 0;
        private int m_MPCost = 0;

        public BattleUnitSkill(int id, int casterId, int targetId) : base(id, casterId, targetId)
        {
            DRBattleUnitSkill drSkill = GameEntry.DataTable.GetDataRow<DRBattleUnitSkill>(id);

            m_Name          = drSkill.Name;
            m_MPCost        = drSkill.MPCost;
            m_DamageRate    = drSkill.DamageRate;
            m_ReleaseRange  = drSkill.ReleaseRange;

            m_Caster        = GameEntry.Entity.GetGameEntity<BattleUnit>(casterId).BattleUnitData;
            m_Target        = GameEntry.Entity.GetGameEntity<BattleUnit>(targetId).BattleUnitData;
        }

        public bool Release()
        {
            if (m_Caster.MP < m_MPCost)
            {
                return false;
            }

            m_Caster.MP -= m_MPCost;
            OnRelease();
            return true;
        }

        protected override void OnRelease()
        {
            int damageHP = Mathf.CeilToInt(m_Caster.ATK * m_DamageRate);
            DamageInfo damageInfo = new DamageInfo(damageHP, m_CasterId, m_TargetId);
            GameEntry.Event.Fire(this, GridUnitDamageEventArgs.Create(damageInfo));
        }
    }
}
