using System;
using UnityEngine;
using GameFramework.DataTable;
using System.Collections.Generic;

namespace SSRPG
{
    [Serializable]
    public class BattleUnitData : GridUnitData
    {
        [SerializeField]
        private int m_MaxHP = 0;

        [SerializeField]
        private int m_MaxMP = 0;

        [SerializeField]
        private int m_Mp = 0;

        [SerializeField]
        private int m_ATK = 0;

        [SerializeField]
        private int m_AtkRange = 0;

        [SerializeField]
        private int m_MOV = 0;

        [SerializeField]
        private List<int> m_SkillList = null;

        public BattleUnitData(int typeId, int parentId, Vector2Int gridPos, CampType campType)
            : base(typeId, parentId, gridPos, GridUnitType.BattleUnit, campType) 
        {
            //DRBattleUnit drBattleUnit = GameEntry.DataTable.GetDataRow<DRBattleUnit>(typeId);

            //m_ATK = drBattleUnit.ATK;
            //m_MOV = drBattleUnit.MOV;
            //m_MaxHP = drBattleUnit.MaxHP;
            //m_MaxMP = drBattleUnit.MaxMP;
            //m_AtkRange = drBattleUnit.AtkRange;
            //Name = drBattleUnit.Name;

            var cfg = GameEntry.Cfg.Tables.TblBattleUnit.Get(typeId);
            m_ATK = cfg.ATK;
            m_MOV = cfg.MOV;
            m_MaxHP = cfg.MaxHP;
            m_MaxMP = cfg.MaxMP;
            m_AtkRange = cfg.AtkRange;
            m_SkillList = cfg.SkillList;
            Name = cfg.Name;

            HP = MaxHP;
            MP = MaxMP;
        }

        /// <summary>
        /// 最大生命。
        /// </summary>
        public override int MaxHP => m_MaxHP;

        /// <summary>
        /// 最大魔法值
        /// </summary>
        public int MaxMP => m_MaxMP;

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
        /// 攻击力
        /// </summary>
        public int ATK => m_ATK;

        /// <summary>
        /// 移动力
        /// </summary>
        public int MOV => m_MOV;

        /// <summary>
        /// 攻击范围
        /// </summary>
        public int AtkRange => m_AtkRange;
    }
}