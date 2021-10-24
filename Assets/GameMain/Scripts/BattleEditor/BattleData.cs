using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace SSRPG
{
    public class BattleData
    {
        public int maxPlayerBattleUnit = 0;
        public List<Vector2Int> playerBrithList = null;
        public Dictionary<int, int> enemyList = null;
        public GridMapData gridMapData = null;

        [JsonConstructor]
        private BattleData()
        {
        }

        public BattleData(GridMapData gridMapData)
        {
            this.gridMapData = gridMapData;
            maxPlayerBattleUnit = 2;
            playerBrithList = new List<Vector2Int>();
            enemyList = new Dictionary<int, int>();
        }
    }
}