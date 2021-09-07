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

        [SerializeField]
        private GridUnit m_GridUnit;

        public GridData(Vector2Int gridPos, GridType gridType)
        {
            m_GridPos = gridPos;
            m_GridType = gridType;
            m_GridUnit = null;
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

        /// <summary>
        /// 占据网格的单位
        /// </summary>
        public GridUnit GridUnit
        {
            get
            {
                return m_GridUnit;
            }
        }

        public void OnGridUnitEnter(GridUnit gridUnit)
        {
            m_GridUnit = gridUnit;
        }

        public void OnGridUnitLeave()
        {
            m_GridUnit = null;
        }
    }
}