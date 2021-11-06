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

        public bool IsDead => m_Data.HP <= 0;

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

        //-----------------------------------

        public void BeAttack(int damageHP)
        {
            damageHP = Mathf.Max(0, damageHP);

            m_Data.HP -= damageHP;
            if (IsDead)
            {
                OnDead();
            }
        }

        protected virtual void OnDead()
        {
            Log.Info("{0}: 死亡", Name);

            GameEntry.Event.Fire(this, EventName.GridUnitDead);
        }
    }
}
