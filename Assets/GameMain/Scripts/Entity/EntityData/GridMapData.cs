using System;
using UnityEngine;
using GameFramework.DataTable;

namespace SSRPG
{
    [Serializable]
    public class GridMapData : EntityData
    {
        [SerializeField]
        private int m_Width = 0;

        [SerializeField]
        private int m_Height = 0;

        public GridMapData(int entityId, int typeId) : base(entityId, typeId)
        {
            IDataTable<DRGridMap> dtGridMaps = GameEntry.DataTable.GetDataTable<DRGridMap>();
            DRGridMap dRGridMap = dtGridMaps.GetDataRow(TypeId);
            if (dRGridMap == null)
            {
                return;
            }

            m_Width = dRGridMap.Width;
            m_Height = dRGridMap.Height;
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
    }
}