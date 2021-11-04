using System;
using UnityEngine;
using UnityGameFramework.Runtime;
using Newtonsoft.Json;

namespace SSRPG
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class GridData
    {
        [JsonProperty]
        [SerializeField]
        private Vector2Int m_GridPos;

        [JsonProperty]
        [SerializeField]
        private GridType m_GridType;

        [SerializeField]
        private GridUnit m_GridUnit;

        [JsonConstructor]
        public GridData(Vector2Int gridPos, GridType gridType)
        {
            m_GridPos = gridPos;
            m_GridType = gridType;
            m_GridUnit = null;
        }

        /// <summary>
        /// 网格位置
        /// </summary>
        public Vector2Int GridPos => m_GridPos;

        /// <summary>
        /// 网格类型
        /// </summary>
        public GridType GridType
        {
            get => m_GridType;
            set => m_GridType = value;
        }

        /// <summary>
        /// 占据的网格单位
        /// </summary>
        public GridUnit GridUnit => m_GridUnit;

        public void OnGridUnitEnter(GridUnit gridUnit)
        {
            if (m_GridUnit != null)
            {
                Log.Warning("单元格已经被占据,无法进入!");
                return;
            }

            m_GridUnit = gridUnit;
        }

        public void OnGridUnitLeave()
        {
            if (m_GridUnit == null)
            {
                Log.Warning("没有单位占据该单元格!");
                return;
            }

            m_GridUnit = null;
        }

        /// <summary>
        /// 是否可以经过
        /// </summary>
        /// <param name="战斗单位数据"></param>
        public bool CanAcross(BattleUnit battleUnit)
        {
            if (battleUnit == null)
            {
                return false;
            }

            if (GridType == GridType.Obstacle)
            {
                return false;
            }

            if (GridUnit != null && GridUnit.Data.CampType != battleUnit.Data.CampType)
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
            if (GridType == GridType.Obstacle || GridUnit != null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否可以攻击
        /// </summary>
        public bool CanAttack()
        {
            if (GridType == GridType.Obstacle)
            {
                return false;
            }

            return true;
        }
    }
}