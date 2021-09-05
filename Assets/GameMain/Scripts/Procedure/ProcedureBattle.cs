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

            int battleId = 1;
            string path = AssetUtl.GetBattleDataPath(battleId);
            BattleData battleData = AssetUtl.LoadJsonData<BattleData>(path);

            GridMapData gridMapData = new GridMapData(battleData.mapId);
            GameEntry.Entity.ShowGridMap(gridMapData);

            for(int i=0; i<battleData.enemyIds.Count; ++i)
            {
                int typeId = battleData.enemyIds[i];
                Vector2Int pos = battleData.enemyPos[i];

                BattleUnitData battleUnitData = new BattleUnitData(typeId, gridMapData.Id, pos, CampType.Enemy);
                GameEntry.Entity.ShowBattleUnit(battleUnitData);
            }
        }
    }
}
