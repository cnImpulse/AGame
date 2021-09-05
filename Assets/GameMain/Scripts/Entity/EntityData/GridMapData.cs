using System;
using UnityEngine;
using GameFramework.DataTable;
using System.Collections.Generic;

namespace SSRPG
{
    [Serializable]
    public class GridMapData : EntityData
    {
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
    }
}