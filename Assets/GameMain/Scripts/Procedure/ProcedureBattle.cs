using UnityEngine;
using GameFramework;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace SSRPG
{
    public class ProcedureBattle : ProcedureBase
    {
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("进入战斗。");
            GameEntry.Entity.ShowGridMap(new GridMapData(GameEntry.Entity.GenerateSerialId(), 10000));
            GameEntry.Entity.ShowBattleUnit(new BattleUnitData(GameEntry.Entity.GenerateSerialId(), 20000, Vector2Int.zero, CampType.Player));
        }
    }
}
