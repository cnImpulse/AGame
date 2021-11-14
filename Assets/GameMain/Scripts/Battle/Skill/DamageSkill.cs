using Cfg.Battle;
using UnityEngine;

namespace SSRPG
{
    public class DamageSkill : Skill
    {
        public DamageSkill(int id, int casterId, int targetId) : base(id, casterId, targetId)
        {
        }

        protected override void OnRelease()
        {
            base.OnRelease();

            Attribute caster = m_Caster.Data.Attribute;
            Attribute target = m_Target.Data.Attribute;

            int effectValue = m_Power + (m_EffectType == EffectType.Physics ? caster.STR - target.DEF : caster.SPR - target.RES);
            effectValue = Mathf.Max(0, effectValue);
            SkillInfo skillInfo = new SkillInfo(-effectValue, m_Caster.Id, m_Target.Id);
            GameEntry.Event.Fire(this, EventName.ReleaseSkill, skillInfo);
        }
    }
}
