using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSRPG
{
    public class MapData
    {
        public int MapId = 0;
        public int Width = 0;
        public int Height = 0;
        public Dictionary<int, GridData> GridList = null;

        public MapData(int width, int height)
        {
            Width = width;
            Height = height;
            GridList = new Dictionary<int, GridData>();
        }
    }
}