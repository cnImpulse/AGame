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
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(parentEntity, parentTransform, userData);

            GridMap ownerMap = parentEntity as GridMap;
            if (ownerMap == null)
            {
                return;
            }
            m_Data.Position = ownerMap.GridPosToWorldPos(m_Data.GridPos);
        }
    }
}
