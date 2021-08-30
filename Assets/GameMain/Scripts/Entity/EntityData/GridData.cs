using System;
using UnityEngine;

namespace SSRPG
{
    [Serializable]
    public class GridData
    {
        [SerializeField]
        private Vector2Int m_GridPos;

        [SerializeField]
        private GridType m_GridType;

        public GridData(Vector2Int gridPos, GridType gridType)
        {
            m_GridPos = gridPos;
            m_GridType = gridType;
        }

        /// <summary>
        /// 网格位置
        /// </summary>
        public Vector2Int GridPos
        {
            get
            {
                return m_GridPos;
            }
        }

        /// <summary>
        /// 网格类型
        /// </summary>
        public GridType GridType
        {
            get
            {
                return m_GridType;
            }
        }
    }
}