using System;
using UnityEngine;

namespace SSRPG
{
    [Serializable]
    public abstract class GridUnitData : ChirldEntityData
    {
        [SerializeField]
        private Vector2Int m_GridPos;

        [SerializeField]
        private GridUnitType m_GridUnitType;

        [SerializeField]
        private CampType m_CampType;

        [SerializeField]
        private int m_HP = 0;

        public GridUnitData(int typeId, int parentId, Vector2Int gridPos, GridUnitType gridUnitType, CampType campType)
            : base(typeId, parentId)
        {
            m_GridPos       = gridPos;
            m_GridUnitType  = gridUnitType;
            m_CampType      = campType;
            m_HP            = MaxHP;
            Name            = gridUnitType.ToString();
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
        public GridUnitType GridUnitType
        {
            get
            {
                return m_GridUnitType;
            }
        }

        /// <summary>
        /// 战斗阵营
        /// </summary>
        public CampType CampType
        {
            get
            {
                return m_CampType;
            }
        }

        /// <summary>
        /// 当前生命。
        /// </summary>
        public int HP
        {
            get
            {
                return m_HP;
            }
            set
            {
                if (value > MaxHP)
                {
                    m_HP = MaxHP;
                }
                else
                {
                    m_HP = value;
                }
            }
        }

        /// <summary>
        /// 最大生命。
        /// </summary>
        public abstract int MaxHP
        {
            get;
        }

        /// <summary>
        /// 生命百分比。
        /// </summary>
        public float HPRatio
        {
            get
            {
                return MaxHP > 0 ? (float)HP / MaxHP : 0f;
            }
        }
    }
}