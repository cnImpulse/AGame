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

        /// <summary>
        /// 是否可以经过
        /// </summary>
        /// <param name="战斗单位数据"></param>
        public bool CanAcross(BattleUnitData battleUnitData)
        {
            if (battleUnitData == null)
            {
                return false;
            }

            if (GridType == GridType.Wall)
            {
                return false;
            }

            if (GridUnit != null && GridUnit.GridData.CampType != battleUnitData.CampType)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否可以到达
        /// </summary>
        public bool CanArrive()
        {
            if (GridType == GridType.Wall || GridUnit != null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否可以攻击
        /// </summary>
        public bool CanAttack(BattleUnitData battleUnitData)
        {
            if (battleUnitData == null)
            {
                return false;
            }

            if (GridType == GridType.Wall)
            {
                return false;
            }

            if (GridUnit != null && GridUnit.GridData.CampType == battleUnitData.CampType)
            {
                return false;
            }

            return true;
        }
    }
}