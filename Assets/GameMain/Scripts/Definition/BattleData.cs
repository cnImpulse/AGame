using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSRPG
{
    public class BattleData
    {
        public int mapId = 0;
        public int maxPlayerBattleUnit = 1;
        public List<Vector2Int> playerBrithPos = new List<Vector2Int>();
        public List<int> enemyIds = new List<int>();
        public List<Vector2Int> enemyPos = new List<Vector2Int>();
    }
}