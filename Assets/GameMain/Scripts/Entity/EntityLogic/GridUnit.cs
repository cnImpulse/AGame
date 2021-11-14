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
    public abstract class GridUnit : Entity
    {
        [SerializeField]
        private GridUnitData m_Data = null;

        public GridUnitData Data => m_Data;

        public GridMap GridMap { get; private set; }

        public GridData GridData => GridMap.Data.GetGridData(m_Data.GridPos);

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            InitLayer("GridUnit");
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_Data = userData as GridUnitData;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            GameEntry.Entity.DetachEntity(Id);

            m_Data = null;
            GridMap = null;

            base.OnHide(isShutdown, userData);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            GridMap = parentEntity as GridMap;
            transform.position = GridMap.GridPosToWorldPos(m_Data.GridPos);
            GameEntry.GridUnitInfo.ShowGridUnitInfo(this);
        }

        protected override void OnDetachFrom(EntityLogic parentEntity, object userData)
        {
            base.OnDetachFrom(parentEntity, userData);
        }
    }
}
