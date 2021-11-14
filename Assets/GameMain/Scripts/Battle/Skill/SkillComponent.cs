using GameFramework.Event;
using UnityGameFramework.Runtime;
using Cfg.Battle;

namespace SSRPG
{
    public class SkillComponent : GameFrameworkComponent
    {
        private void Start()
        {
            GameEntry.Event.Subscribe(EventName.ReleaseSkill, OnReleaseSkill);
        }

        private Skill CreatSkill(int skillId, int casterId, int targetId)
        {
            var cfg = GameEntry.Cfg.Tables.TblSkill.Get(skillId);

            if (cfg.SkillType == SkillType.Cure)
            {
                return new CureSkill(skillId, casterId, targetId);
            }
            return new DamageSkill(skillId, casterId, targetId);
        }

        public bool RequestReleaseSkill(int skillId, int casterId, int targetId)
        {
            Skill skill = CreatSkill(skillId, casterId, targetId);
            return skill.Release();
        }

        private void OnReleaseSkill(object sender, GameEventArgs e)
        {
            var ne = e as GameEventBase;
            var info = ne.UserData as SkillInfo;

            GridUnit caster = GameEntry.Entity.GetEntityLogic<GridUnit>(info.CasterId);
            GridUnit target = GameEntry.Entity.GetEntityLogic<GridUnit>(info.TargetId);

            target.Data.HP += info.EffectValue;
            GameEntry.UI.OpenUIForm(Cfg.UI.FormType.TextBubbleForm, info);
            if (info.EffectValue < 0)
            {
                GameEntry.Effect.CreatEffect(Cfg.Effect.EffectType.Attack, target.transform.position, 0.5f);
                GameEntry.GameTips.PlayTips(string.Format("{0}对{1}造成{2}点伤害",
                    BattleUtl.GetText(caster.Data.CampType, caster.Name),
                    BattleUtl.GetText(target.Data.CampType, target.Name),
                    BattleUtl.GetText(caster.Data.CampType, info.EffectValue.ToString())));
            }
        }
    }
}
