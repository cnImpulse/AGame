using Cfg.Battle;

namespace SSRPG
{
    public class CureSkill : Skill
    {
        public CureSkill(int id, int casterId, int targetId) : base(id, casterId, targetId)
        {
        }

        protected override void OnRelease()
        {
            base.OnRelease();

            Attribute caster = m_Caster.Data.Attribute;

            int effectValue = m_Power + (m_EffectType == EffectType.Magic ? caster.SPR : caster.STR);
            SkillInfo skillInfo = new SkillInfo(effectValue, m_Caster.Id, m_Target.Id);
            GameEntry.Event.Fire(this, EventName.ReleaseSkill, skillInfo);
        }
    }
}
