using System;
using UnityEngine;

namespace SSRPG
{
    [Serializable]
    public class GridMapData : EntityData
    {
        [SerializeField]
        private int m_Width = 0;

        [SerializeField]
        private int m_Height = 0;

        public GridMapData(int entityId, int typeId, int width, int height) : base(entityId, typeId)
        {
            m_Width = width;
            m_Height = height;
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
        /// 地图宽。
        /// </summary>
        public int Height
        {
            get
            {
                return Height;
            }
            set
            {
                m_Height = value;
            }
        }
    }
}