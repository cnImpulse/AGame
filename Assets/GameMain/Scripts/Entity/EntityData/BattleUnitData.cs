using System;
using UnityEngine;
using GameFramework.DataTable;

namespace SSRPG
{
    [Serializable]
    public class BattleUnitData : GridUnitData
    {
        [SerializeField]
        private int m_MaxHP = 0;

        [SerializeField]
        private int m_ATK = 0;

        [SerializeField]
        private int m_MOV = 0;

        public BattleUnitData(int typeId, Vector2Int gridPos, CampType campType)
            : base(typeId, gridPos, GridUnitType.BattleUnit, campType) 
        {
            IDataTable<DRBattleUnit> dtBattleUnits = GameEntry.DataTable.GetDataTable<DRBattleUnit>();
            DRBattleUnit drBattleUnit = dtBattleUnits.GetDataRow(typeId);
            if (drBattleUnit == null)
            {
                return;
            }

            m_ATK   = drBattleUnit.ATK;
            m_MOV   = drBattleUnit.MOV;
            m_MaxHP = drBattleUnit.MaxHP;
            Name    = drBattleUnit.Name;
        }

        /// <summary>
        /// 最大生命。
        /// </summary>
        public override int MaxHP
        {
            get
            {
                return m_MaxHP;
            }
        }

        /// <summary>
        /// 攻击力
        /// </summary>
        public int ATK
        {
            get
            {
                return m_ATK;
            }
        }

        /// <summary>
        /// 移动力
        /// </summary>
        public int MOV
        {
            get
            {
                return m_MOV;
            }
        }
    }
}