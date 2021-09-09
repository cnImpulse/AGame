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
                    List<GridData> neighbors = GetNeighbors(gridData, battleUnitData);
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

        // 战斗单位可穿过的邻居
        private List<GridData> GetNeighbors(GridData gridData, BattleUnitData battleUnitData)
        {
            List<GridData> neighbors = new List<GridData>();
            for (int i = 0; i < s_DirArray4.Length; ++i)
            {
                GridData grid = GetGridData(gridData.GridPos + s_DirArray4[i]);
                if (grid == null || !grid.CanAcross(battleUnitData))
                {
                    continue;
                }

                neighbors.Add(grid);
            }

            return neighbors;
        }
    }
}