using System;
using UnityEngine;
using GameFramework.DataTable;
using System.Collections.Generic;

namespace SSRPG
{
    [Serializable]
    public class BattleUnitData : GridUnitData
    {
        private RoleData m_RoleData = null;

        private int m_Mp = 0;

        public BattleUnitData(RoleData roleData, Vector2Int gridPos, CampType campType)
            : base(roleData.TypeId, gridPos, GridUnitType.BattleUnit, campType)
        {
            m_RoleData = roleData;

            Name = roleData.Name;
            HP = MaxHP;
            MP = MaxMP;
        }

        public Attribute Attribute => m_RoleData.Attribute;

        /// <summary>
        /// 最大生命。
        /// </summary>
        public override int MaxHP => Attribute.HP;

        /// <summary>
        /// 最大魔法值
        /// </summary>
        public int MaxMP => Attribute.MP;

        /// <summary>
        /// 魔法值
        /// </summary>
        public int MP
        {
            get => m_Mp;
            set
            {
                if (value > MaxMP)
                {
                    m_Mp = MaxMP;
                }
                else
                {
                    m_Mp = value;
                }
            }
        }

        /// <summary>
        /// 移动力
        /// </summary>
        public int MOV => Attribute.MOV;

        /// <summary>
        /// 技能列表
        /// </summary>
        public List<int> SkillList => m_RoleData.SkillList;
    }
}