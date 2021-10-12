using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class BattleComponent : GameFrameworkComponent
    {
        public bool AutoBattle = true;

        public GridMap GridMap
        {
            get;
            private set;
        }

        private void Start()
        {
            GameEntry.Event.Subscribe(GridUnitDamageEventArgs.EventId, OnGridUnitDamage);
            GameEntry.Event.Subscribe(GridUnitDeadEventArgs.EventId, OnGridUnitDead);
        }

        public void InitBattle(GridMap gridMap)
        {
            GridMap = gridMap;
        }

        private void OnGridUnitDamage(object sender, GameEventArgs e)
        {
            var ne = (GridUnitDamageEventArgs)e;

            GridUnit gridUnit = GameEntry.Entity.GetGameEntity<GridUnit>(ne.DamageInfo.TargetId);
            gridUnit.BeAttack(ne.DamageInfo.DamageHP);
        }

        private void OnGridUnitDead(object sender, GameEventArgs e)
        {
            var ne = (GridUnitDeadEventArgs)e;

            
        }
    }
}
