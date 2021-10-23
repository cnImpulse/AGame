using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSRPG
{
    public class BattleData
    {
        public int MapId = 0;
        public int MaxPlayerBattleUnit = 1;
        public List<Vector2Int> PlayerBrithPos = new List<Vector2Int>();
        public List<int> EnemyIdList = new List<int>();
        public List<Vector2Int> EnemyPosList = new List<Vector2Int>();
    }
}