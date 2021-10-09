using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class BattleComponent : GameFrameworkComponent
    {
        private void Start()
        {
            GameEntry.Event.Subscribe(GridUnitDamageEventArgs.EventId, OnGridUnitDamage);
        }

        private void OnGridUnitDamage(object sender, GameEventArgs e)
        {
            var ne = (GridUnitDamageEventArgs)e;

            GridUnit gridUnit = GameEntry.Entity.GetGameEntity<GridUnit>(ne.DamageInfo.TargetId);
            gridUnit.BeAttack(ne.DamageInfo.DamageHP);
        }
    }
}
