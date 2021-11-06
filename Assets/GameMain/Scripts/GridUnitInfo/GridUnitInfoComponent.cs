using System.Collections.Generic;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class GridUnitInfoComponent : GameFrameworkComponent
    {
        public bool Disable = true;

        [SerializeField]
        private GridUnitInfoItem m_Template = null;

        [SerializeField]
        private Transform m_InstanceRoot = null;

        [SerializeField]
        private int m_InstancePoolCapacity = 16;

        private IObjectPool<GridUnitInfoItemObject> m_ObjectPool = null;
        private List<GridUnitInfoItem> m_ActiveItems = null;
        private Canvas m_CachedCanvas = null;

        private void Start()
        {
            if (m_InstanceRoot == null)
            {
                Log.Error("You must set instance root first.");
                return;
            }

            m_CachedCanvas = m_InstanceRoot.GetComponent<Canvas>();
            m_ObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<GridUnitInfoItemObject>("GridUnitInfoItem", m_InstancePoolCapacity);
            m_ActiveItems = new List<GridUnitInfoItem>();
        }

        private void OnDestroy()
        {
        }

        private void Update()
        {
            for (int i = m_ActiveItems.Count - 1; i >= 0; i--)
            {
                var item = m_ActiveItems[i];
                if (item.Refresh())
                {
                    continue;
                }

                HideItem(item);
            }
        }

        public void ShowGridUnitInfo(GridUnit gridUnit)
        {
            if (Disable)
            {
                return;
            }

            if (gridUnit == null)
            {
                Log.Warning("GridUnit is invalid.");
                return;
            }

            var item = GetActiveItem(gridUnit);
            if (item == null)
            {
                item = CreateItem(gridUnit);
                m_ActiveItems.Add(item);
            }

            item.Init(gridUnit, m_CachedCanvas);
        }

        private void HideItem(GridUnitInfoItem item)
        {
            item.Reset();
            m_ActiveItems.Remove(item);
            m_ObjectPool.Unspawn(item);
        }

        private GridUnitInfoItem GetActiveItem(GridUnit gridUnit)
        {
            if (gridUnit == null)
            {
                return null;
            }

            for (int i = 0; i < m_ActiveItems.Count; i++)
            {
                if (m_ActiveItems[i].Owner == gridUnit)
                {
                    return m_ActiveItems[i];
                }
            }

            return null;
        }

        private GridUnitInfoItem CreateItem(Entity entity)
        {
            GridUnitInfoItem item = null;
            GridUnitInfoItemObject itemObject = m_ObjectPool.Spawn();
            if (itemObject != null)
            {
                item = (GridUnitInfoItem)itemObject.Target;
            }
            else
            {
                item = Instantiate(m_Template);
                item.transform.SetParent(m_InstanceRoot);
                transform.localScale = Vector3.one;
                m_ObjectPool.Register(GridUnitInfoItemObject.Create(item), true);
            }

            return item;
        }
    }
}
