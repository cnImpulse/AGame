using System;
using UnityEngine;

namespace SSRPG
{
    [Serializable]
    public abstract class GridUnitData : EntityData
    {
        [SerializeField]
        private Vector2Int m_GridPos;

        [SerializeField]
        private GridUnitType m_GridUnitType;

        [SerializeField]
        private CampType m_CampType;

        [SerializeField]
        private int m_HP = 0;

        public GridUnitData(int typeId, Vector2Int gridPos, GridUnitType gridUnitType, CampType campType)
            : base(typeId)
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
            get => m_GridPos;
            set => m_GridPos = value;
        }

        /// <summary>
        /// 网格类型
        /// </summary>
        public GridUnitType GridUnitType => m_GridUnitType;

        /// <summary>
        /// 战斗阵营
        /// </summary>
        public CampType CampType => m_CampType;

        /// <summary>
        /// 生命值。
        /// </summary>
        public int HP
        {
            get => m_HP;
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