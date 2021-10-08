using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class BattleComponent : GameFrameworkComponent
    {
        protected override void Awake()
        {
            base.Awake();

            GameEntry.Event.Subscribe(GridUnitDamageEventArgs.EventId, OnGridUnitDamage);
        }

        private void OnGridUnitDamage(object sender, GameEventArgs e)
        {
            GridUnitDamageEventArgs ne = (GridUnitDamageEventArgs)e;

            GridUnitData gridUnitData = GameEntry.Entity.GetGameEntity<GridUnit>(ne.DamageInfo.TargetId).GridUnitData;
            gridUnitData.HP -= ne.DamageInfo.DamageHP;
        }
    }
}
