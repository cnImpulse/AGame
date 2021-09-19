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

        protected GridMap m_GridMap;

        public bool IsDead
        {
            get
            {
                return m_Data.HP <= 0;
            }
        }

        public virtual GridUnitData GridUnitData
        {
            get
            {
                return m_Data;
            }
        }

        public GridUnitType GridUnitType
        {
            get
            {
                return m_Data.GridUnitType;
            }
        }

        public CampType CampType
        {
            get
            {
                return m_Data.CampType;
            }
        }

        public Vector2Int GridPos
        {
            get
            {
                return m_Data.GridPos;
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

            GameEntry.Entity.AttachEntity(Entity, m_Data.ParentId);
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
        }

        public void BeAttack(int atk)
        {
            atk = Mathf.Max(0, atk);

            m_Data.HP -= atk;
            Log.Info("{0}: 被攻击。生命值: {1}", Name, m_Data.HP);

            if (IsDead)
            {
                OnDead();
            }
        }

        protected virtual void OnDead()
        {
            Log.Info("{0}: 死亡", Name);

            m_GridMap.UnRegisterGridUnit(this);

            GameEntry.Event.Fire(this, GridUnitDeadEventArgs.Create(this));
        }

        public GridData GridData
        {
            get
            {
                return m_GridMap.GridMapData.GetGridData(m_Data.GridPos);
            }
        }
    }
}
