using System;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class BattleComponent : GameFrameworkComponent
    {
        public int BattleId = 2;
        public bool AutoBattle = true;

        private int m_SelectEffectId = 0;

        private void Start()
        {
            GameEntry.Event.Subscribe(EventName.GridUnitDamage, OnGridUnitDamage);
            GameEntry.Event.Subscribe(EventName.GridUnitDead, OnGridUnitDead);
        }

        public void ShowSelectEffect(Vector3 position)
        {
            var entity = GameEntry.Effect.GetEffect(m_SelectEffectId);
            if (entity == null)
            {
                m_SelectEffectId = GameEntry.Effect.CreatEffect(Cfg.Effect.EffectType.Select, position);
            }
            else
            {
                entity.transform.position = position;
            }
        }

        public void HideSelectEffect()
        {
            GameEntry.Effect.HideEffect(m_SelectEffectId);
            m_SelectEffectId = 0;
        }

        private void OnGridUnitDamage(object sender, GameEventArgs e)
        {
            var ne = e as GameEventBase;
            var info = ne.UserData as DamageInfo;

            GridUnit caster = GameEntry.Entity.GetEntityLogic<GridUnit>(info.CasterId);
            GridUnit target = GameEntry.Entity.GetEntityLogic<GridUnit>(info.TargetId);
            target.BeAttack(info.DamageHP);

            GameEntry.Effect.CreatEffect(Cfg.Effect.EffectType.Attack, target.transform.position, 0.5f);
            GameEntry.GameTips.PlayTips(string.Format("{0}对{1}造成<color=#FF7070>{2}</color>点伤害",
                BattleUtl.GetNameText(caster), BattleUtl.GetNameText(caster), info.DamageHP));
        }

        private void OnGridUnitDead(object sender, GameEventArgs e)
        {
            var ne = e as GameEventBase;

        }
    }
}
