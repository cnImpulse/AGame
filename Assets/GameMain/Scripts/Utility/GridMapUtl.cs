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

                    gridList.Add(gridIndex, new GridData(gridPos, GridType.Normal));
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