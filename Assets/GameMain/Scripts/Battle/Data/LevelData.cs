using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace SSRPG
{
    public class LevelData
    {
        public int MapId = 0;
        public int MapLevel = 0;
        public int MaxPlayerBattleUnit = -1;
        public List<Vector2Int> PlayerBrithList = null;
        public Dictionary<int, int> EnemyList = null;

        [JsonConstructor]
        private LevelData()
        {
        }

        public LevelData(int mapId, int mapLevel)
        {
            MapId = mapId;
            MapLevel = mapLevel;
            PlayerBrithList = new List<Vector2Int>();
            EnemyList = new Dictionary<int, int>();
        }
    }
}