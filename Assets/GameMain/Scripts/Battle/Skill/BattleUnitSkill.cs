using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;

namespace SSRPG
{
    public class BattleUnitSkill : SkillBase
    {
        private BattleUnit m_Caster = null;
        private BattleUnit m_Target = null;

        private string m_Name = null;
        private float m_DamageRate = 0;
        private int m_ReleaseRange = 0;
        private int m_MPCost = 0;

        public BattleUnitSkill(int id, int casterId, int targetId) : base(id, casterId, targetId)
        {
            var cfg = GameEntry.Cfg.Tables.TblBattleUnitSkill.Get(id);

            m_Name          = cfg.Name;
            m_MPCost        = cfg.MPCost;
            m_DamageRate    = cfg.DamageRate;
            m_ReleaseRange  = cfg.ReleaseRange;

            m_Caster        = GameEntry.Entity.GetEntityLogic<BattleUnit>(casterId);
            m_Target        = GameEntry.Entity.GetEntityLogic<BattleUnit>(targetId);
        }

        public bool Release()
        {
            if (m_Caster.Data.MP < m_MPCost)
            {
                GameEntry.GameTips.PlayTips("法力值不足");
                return false;
            }

            m_Caster.Data.MP -= m_MPCost;
            OnRelease();
            return true;
        }

        protected override void OnRelease()
        {
            int damageHP = Mathf.CeilToInt(m_Caster.Data.ATK * m_DamageRate);
            DamageInfo damageInfo = new DamageInfo(damageHP, m_CasterId, m_TargetId);
            GameEntry.Event.Fire(this, EventName.GridUnitDamage, damageInfo);

            string tips = string.Format("{0}对{1}释放技能{2}",
                BattleUtl.GetText(m_Caster.Data.CampType, m_Caster.Name),
                BattleUtl.GetText(m_Target.Data.CampType, m_Target.Name),
                BattleUtl.GetText(m_Caster.Data.CampType, m_Name));
            GameEntry.GameTips.PlayTips(tips);
        }
    }
}
