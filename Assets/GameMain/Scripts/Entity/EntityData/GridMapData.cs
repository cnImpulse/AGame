using System;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SSRPG
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class GridMapData : EntityData
    {
        public static Vector2Int[] s_DirArray4 = { Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.right };
        public static Vector2Int[] s_Dir2Array4 = { Vector2Int.one, new Vector2Int(1, -1), new Vector2Int(-1, -1), new Vector2Int(-1, 1) };

        [JsonProperty]
        private Dictionary<int, GridData> m_GridList = null;

        [JsonConstructor]
        public GridMapData(int mapId) : base(mapId)
        {
            Name = "GridMap";
            m_GridList = new Dictionary<int, GridData>();
        }

        public GridMapData(int width, int height, int mapId) : base(mapId)
        {
            Name = "GridMap";
            m_GridList = new Dictionary<int, GridData>();
            for (int x = -width / 2; x < width / 2; ++x)
            {
                for (int y = -height / 2; y < height / 2; ++y)
                {
                    var pos = new Vector2Int(x, y);
                    int index = GridMapUtl.GridPosToIndex(pos);
                    m_GridList[index] = new GridData(pos, GridType.Land);
                }
            }
        }

        /// <summary>
        /// 获得网格数据列表
        /// </summary>
        public Dictionary<int, GridData> GridList => m_GridList;



        public void SetGridData(GridData gridData)
        {
            var index = GridMapUtl.GridPosToIndex(gridData.GridPos);
            m_GridList[index] = gridData;
        }

        public GridData GetGridData(Vector2Int gridPos)
        {
            int index = GridMapUtl.GridPosToIndex(gridPos);
            return GetGridData(index);
        }

        public GridData GetGridData(int gridIndex)
        {
            if (GridList.TryGetValue(gridIndex, out var gridData))
            {
                return gridData;
            }
            return null;
        }

        // 广度优先搜索
        public List<GridData> GetCanMoveGrids(BattleUnit battleUnit)
        {
            if (battleUnit == null)
            {
                return null;
            }

            int mov = battleUnit.Data.MOV;
            GridData start = GetGridData(battleUnit.Data.GridPos);

            Queue<GridData> open = new Queue<GridData>();
            List<GridData> close = new List<GridData>();

            open.Enqueue(start);
            for (int i = 0; i <= mov; ++i)
            {
                int length = open.Count;
                if (length == 0)
                {
                    break;
                }

                for (int j = 0; j < length; ++j)
                {
                    GridData gridData = open.Dequeue();
                    List<GridData> neighbors = GetNeighbors(gridData.GridPos, battleUnit, NeighborType.CanAcross);
                    foreach (var neighbor in neighbors)
                    {
                        if (!close.Contains(neighbor) && !open.Contains(neighbor)) 
                        {
                            open.Enqueue(neighbor);
                        }
                    }
                    close.Add(gridData);
                }
            }

            // 排除被占据的单元格
            List<GridData> canMoveList = new List<GridData>();
            foreach (var grid in close)
            {
                if (grid.CanArrive())
                {
                    canMoveList.Add(grid);
                }
            }

            return canMoveList;
        }

        public List<GridData> GetCanAttackGrids(BattleUnit battleUnit, int atkRange, bool beforeMove = false)
        {
            if (beforeMove == false)
            {
                return GetCanAttackGrids(battleUnit.Data.GridPos, atkRange);
            }

            List<GridData> canMoveList = GetCanMoveGrids(battleUnit);
            List<GridData> canAttackList = new List<GridData>();

            foreach (var gridData in canMoveList)
            {
                var gridList = GetCanAttackGrids(gridData.GridPos, atkRange);
                foreach (var grid in gridList)
                {
                    if (canMoveList.Contains(grid) || canAttackList.Contains(grid))
                    {
                        continue;
                    }

                    canAttackList.Add(grid);
                }
            }

            return canAttackList;
        }

        public List<GridData> GetSkillReleaseRange(BattleUnit battleUnit, int skillId)
        {
            var cfg = GameEntry.Cfg.Tables.TblSkill.Get(skillId);
            return GetCanAttackGrids(battleUnit.Data.GridPos, cfg.ReleaseRange);
        }

        private List<GridData> GetCanAttackGrids(Vector2Int pos, int atkRange)
        {
            List<GridData> canAttackList = new List<GridData>();
            List<GridData> gridList = GetRangeGridList(pos, atkRange);
            foreach (var gridData in gridList)
            {
                if (gridData.CanAttack())
                {
                    canAttackList.Add(gridData);
                }
            }

            return canAttackList;
        }

        // 菱形遍历
        public List<GridData> GetRangeGridList(Vector2Int centerPos, int range)
        {
            GridData center = GetGridData(centerPos);
            List<GridData> gridList = new List<GridData>();

            for (int i = 1; i <= range; ++i)
            {
                Vector2Int position = new Vector2Int(-i, 0);
                for (int k = 0; k < s_Dir2Array4.Length; ++k)
                {
                    for (int j = 0; j < i; ++j)
                    {
                        GridData gridData = GetGridData(position + center.GridPos);
                        position += s_Dir2Array4[k];
                        if (gridData != null)
                        {
                            gridList.Add(gridData); ;
                        }
                    }
                }
            }

            return gridList;
        }

        // 战斗单位可穿过的邻居
        public List<GridData> GetNeighbors(Vector2Int centerPos , BattleUnit battleUnit, NeighborType type = NeighborType.None)
        {
            GridData gridData = GetGridData(centerPos);
            List<GridData> neighbors = new List<GridData>();
            for (int i = 0; i < s_DirArray4.Length; ++i)
            {
                GridData grid = GetGridData(gridData.GridPos + s_DirArray4[i]);
                if (grid == null)
                {
                    continue;
                }

                bool flag = true;
                switch (type)
                {
                    case NeighborType.CanArrive: flag = grid.CanArrive(); break;
                    case NeighborType.CanAcross: flag = grid.CanAcross(battleUnit); break;
                    case NeighborType.CanAttack: flag = grid.CanAttack(); break;
                }

                if (flag == false)
                {
                    continue;
                }

                neighbors.Add(grid);
            }

            return neighbors;
        }

        public List<GridData> GetBoxGridList(Vector2Int start, Vector2Int end)
        {
            var leftLower = new Vector2Int(Mathf.Min(start.x, end.x), Mathf.Min(start.y, end.y));
            var rightTop = new Vector2Int(Mathf.Max(start.x, end.x), Mathf.Max(start.y, end.y));

            var gridList = new List<GridData>();
            for (int x = leftLower.x; x <= rightTop.x; ++x)
            {
                for (int y = leftLower.y; y <= rightTop.y; ++y)
                {
                    var gridData = GetGridData(new Vector2Int(x, y));
                    if (gridData != null)
                    {
                        gridList.Add(gridData);
                    }
                }
            }
            return gridList;
        }
    }
}