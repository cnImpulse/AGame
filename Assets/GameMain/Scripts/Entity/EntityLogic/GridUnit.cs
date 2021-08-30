using GameFramework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 网格单位。
    /// </summary>
    public class GridUnit : Entity
    {
        [SerializeField]
        private GridUnitData m_Data;

        public bool IsDead
        {
            get
            {
                return m_Data.HP <= 0;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            gameObject.SetLayerRecursively(Constant.Layer.GridUnitLayerId);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_Data = userData as GridUnitData;
            if (m_Data == null)
            {
                Log.Error("GridUnit object data is invalid.");
                return;
            }
        }
    }
}
