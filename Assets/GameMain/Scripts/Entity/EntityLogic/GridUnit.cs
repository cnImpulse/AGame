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
        private GridUnitData m_Data;

        protected GridMap m_GridMap;

        public GridUnitData Data => m_Data;

        public bool IsDead => m_Data.HP <= 0;

        public GridData GridData => m_GridMap.Data.GetGridData(m_Data.GridPos);

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

            GameEntry.Entity.AttachEntity(Entity, m_Data.ParentId);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            m_Data = null;
            m_GridMap = null;
            GameEntry.Entity.DetachEntity(Id);

            base.OnHide(isShutdown, userData);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            m_GridMap = parentEntity as GridMap;
            if (m_GridMap == null)
            {
                return;
            }

            transform.position = m_GridMap.GridPosToWorldPos(m_Data.GridPos);
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

            GameEntry.Event.Fire(this, GridUnitDeadEventArgs.Create(this));
        }
    }
}
