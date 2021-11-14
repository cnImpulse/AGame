using Cfg.Battle;

namespace SSRPG
{
    public abstract class Skill
    {
        protected int m_Id = 0;
        protected int m_Power = 0;
        protected int m_MpCost = 0;
        protected int m_ReleaseRange = 0;
        protected string m_Name = null;
        protected EffectType m_EffectType = default;

        protected BattleUnit m_Caster = null;
        protected BattleUnit m_Target = null;

        public Skill(int id, int casterId, int targetId)
        {
            var cfg = GameEntry.Cfg.Tables.TblSkill.Get(id);

            m_Id = id;
            m_Power = cfg.Power;
            m_MpCost = cfg.MpCost;
            m_ReleaseRange = cfg.ReleaseRange;
            m_EffectType = cfg.EffectType;

            m_Caster = GameEntry.Entity.GetEntityLogic<BattleUnit>(casterId);
            m_Target = GameEntry.Entity.GetEntityLogic<BattleUnit>(targetId);
        }

        public bool Release()
        {
            if (m_Caster.Data.MP < m_MpCost)
            {
                GameEntry.GameTips.PlayTips("法力值不足");
                return false;
            }

            m_Caster.Data.MP -= m_MpCost;
            OnRelease();
            return true;
        }

        protected virtual void OnRelease()
        {
            string tips = string.Format("{0}对{1}释放技能{2}",
                BattleUtl.GetText(m_Caster.Data.CampType, m_Caster.Name),
                BattleUtl.GetText(m_Target.Data.CampType, m_Target.Name),
                BattleUtl.GetText(m_Caster.Data.CampType, m_Name));
            GameEntry.GameTips.PlayTips(tips);
        }
    }
}
