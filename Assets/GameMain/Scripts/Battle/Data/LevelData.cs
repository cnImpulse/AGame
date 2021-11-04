using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace SSRPG
{
    public class LevelData
    {
        public int mapId = 0;
        public int maxPlayerBattleUnit = -1;
        public HashSet<Vector2Int> playerBrithList = null;
        public Dictionary<int, int> enemyList = null;

        [JsonConstructor]
        private LevelData()
        {
        }

        public LevelData(int mapId)
        {
            this.mapId = mapId;
            playerBrithList = new HashSet<Vector2Int>();
            enemyList = new Dictionary<int, int>();
        }
    }
}