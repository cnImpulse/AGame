using System;
using System.Collections.Generic;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class BattleComponent : GameFrameworkComponent
    {
        public int BattleId = 2;
        public bool AutoBattle = true;

        [NonSerialized]
        public int ActionArg = 0;
        [NonSerialized]
        public BattleUnit SelectBattleUnit = null;

        public GridMap GridMap
        {
            get;
            private set;
        }

        private void Start()
        {
            GameEntry.Event.Subscribe(EventName.GridUnitDamage, OnGridUnitDamage);
            GameEntry.Event.Subscribe(GridUnitDeadEventArgs.EventId, OnGridUnitDead);
        }

        public void InitBattle(GridMap gridMap)
        {
            GridMap = gridMap;
        }

        private void OnGridUnitDamage(object sender, GameEventArgs e)
        {
            var ne = e as GameEventBase;
            var info = ne.UserData as DamageInfo;

            GridUnit caster = GameEntry.Entity.GetEntityLogic<GridUnit>(info.CasterId);
            GridUnit target = GameEntry.Entity.GetEntityLogic<GridUnit>(info.TargetId);
            target.BeAttack(info.DamageHP);

            GameEntry.GameTips.PlayTips(string.Format("{0}对{1}造成<color=#FF7070>{2}</color>点伤害",
                Utl.GetText(caster.Data.CampType, caster.Name), Utl.GetText(target.Data.CampType, target.Name), info.DamageHP));
        }

        private void OnGridUnitDead(object sender, GameEventArgs e)
        {
            var ne = (GridUnitDeadEventArgs)e;

            
        }
    }
}
