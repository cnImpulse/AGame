using System;
using UnityEngine;
using GameFramework.DataTable;
using System.Collections.Generic;

namespace SSRPG
{
    [Serializable]
    public class GridMapData : EntityData
    {
        public static Vector2Int[] s_DirArray4 = { Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.right };
        public static Vector2Int[] s_Dir2Array4 = { Vector2Int.one, new Vector2Int(1, -1), new Vector2Int(-1, -1), new Vector2Int(-1, 1) };

        [SerializeField]
        private int m_Width = 0;

        [SerializeField]
        private int m_Height = 0;

        [SerializeField]
        private Dictionary<int, GridData> m_GridList = null;

        public GridMapData(int typeId) : base(typeId)
        {
            string path = AssetUtl.GetMapDataPath(typeId);
            MapData mapData = AssetUtl.LoadJsonData<MapData>(path);

            m_Width = mapData.Width;
            m_Height = mapData.Height;
            m_GridList = mapData.GridList;
            Name = "GridMap";
        }

        /// <summary>
        /// 地图宽。
        /// </summary>
        public int Width
        {
            get
            {
                return m_Width;
            }
            set
            {
                m_Width = value;
            }
        }

        /// <summary>
        /// 地图高。
        /// </summary>
        public int Height
        {
            get
            {
                return m_Height;
            }
            set
            {
                m_Height = value;
            }
        }

        /// <summary>
        /// 获得网格数据列表
        /// </summary>
        public Dictionary<int, GridData> GridList
        {
            get
            {
                return m_GridList;
            }
        }

        public int GetGridIndex(Vector2Int gridPos)
        {
            return gridPos.y * Width + gridPos.x;
        }

        public GridData GetGridData(Vector2Int gridPos)
        {
            int gridIndex = GetGridIndex(gridPos);
            return GetGridData(gridIndex);
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
        public List<GridData> GetCanMoveGrids(BattleUnitData battleUnitData)
        {
            if (battleUnitData == null)
            {
                return null;
            }

            int mov = battleUnitData.MOV;
            GridData start = GetGridData(battleUnitData.GridPos);

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
                    List<GridData> neighbors = GetNeighbors(gridData.GridPos, battleUnitData, NeighborType.CanAcross);
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

        public List<GridData> GetCanAttackGrids(BattleUnitData battleUnitData, bool beforeMove = false)
        {
            if (beforeMove == false)
            {
                return GetCanAttackGrids(battleUnitData.GridPos, battleUnitData.AtkRange);
            }

            List<GridData> canMoveList = GetCanMoveGrids(battleUnitData);
            List<GridData> canAttackList = new List<GridData>();

            foreach (var gridData in canMoveList)
            {
                var gridList = GetCanAttackGrids(gridData.GridPos, battleUnitData.AtkRange);
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
        public List<GridData> GetNeighbors(Vector2Int centerPos , BattleUnitData battleUnitData, NeighborType type = NeighborType.None)
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
                    case NeighborType.CanAcross: flag = grid.CanAcross(battleUnitData); break;
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
    }
}