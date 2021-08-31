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

        public static int GetGridIndex(int width, Vector2Int gridPos)
        {
            return gridPos.y * width + gridPos.x;
        }
    }
}