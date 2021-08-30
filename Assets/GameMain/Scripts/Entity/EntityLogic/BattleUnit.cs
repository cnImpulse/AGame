using GameFramework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 战斗单位。
    /// </summary>
    public class BattleUnit : GridUnit
    {
        [SerializeField]
        private BattleUnitData m_Data;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_Data = userData as BattleUnitData;
            if (m_Data == null)
            {
                Log.Error("BattleUnit object data is invalid.");
                return;
            }

            Name = Utility.Text.Format("[{0} {1}]", m_Data.Name, Id);
        }
    }
}
