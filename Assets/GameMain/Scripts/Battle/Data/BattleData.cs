using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace SSRPG
{
    public class BattleData
    {
        public int maxPlayerBattleUnit = 0;
        public HashSet<Vector2Int> playerBrithList = null;
        public Dictionary<int, int> enemyList = null;
        public GridMapData gridMapData = null;

        [JsonConstructor]
        private BattleData()
        {
        }

        public BattleData(GridMapData gridMapData)
        {
            this.gridMapData = gridMapData;
            maxPlayerBattleUnit = 4;
            playerBrithList = new HashSet<Vector2Int>();
            enemyList = new Dictionary<int, int>();
        }
    }
}