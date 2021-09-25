using GameFramework;
using UnityEngine;
using System.Collections.Generic;

namespace SSRPG
{
    public static class GridMapUtl
    {
        public static Dictionary<int, GridData> GenerateGridList(int width, int height)
        {
            Dictionary<int, GridData> gridList = new Dictionary<int, GridData>();
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    Vector2Int gridPos = new Vector2Int(i, j);
                    int gridIndex = GetGridIndex(width, gridPos);
                    GridType gridType = GridType.Normal;
                    if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                    {
                        gridType = GridType.Wall;
                    }

                    gridList.Add(gridIndex, new GridData(gridPos, gridType));
                }
            }

            return gridList;
        }

        public static List<Vector2Int> GenerateBrithPlaces(int width, int height)
        {
            List<Vector2Int> brithPlaces = new List<Vector2Int>();
            brithPlaces.Add(new Vector2Int(width / 2, 1));
            return brithPlaces;
        }

        public static int GetGridIndex(int width, Vector2Int gridPos)
        {
            return gridPos.y * width + gridPos.x;
        }

        /// <summary>
        /// 计算曼哈顿距离
        /// </summary>
        public static int GetDistance(Vector2Int pos1, Vector2Int pos2)
        {
            return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y);
        }

        /// <summary>
        /// 计算曼哈顿距离
        /// </summary>
        public static int GetDistance(GridData pos1, GridData pos2)
        {
            return GetDistance(pos1.GridPos, pos2.GridPos);
        }

        /// <summary>
        /// 获得更近的网格数据,相同优先返回pos1
        /// </summary>
        public static GridData GetNearestGridData(GridData target, GridData pos1, GridData pos2)
        {
            int dis1 = GetDistance(target, pos1);
            int dis2 = GetDistance(target, pos2);
            return dis1 <= dis2 ? pos1 : pos2;
        }
    }
}