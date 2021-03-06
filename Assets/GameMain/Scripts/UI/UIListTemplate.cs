using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class UIListTemplate : MonoBehaviour
    {
        [SerializeField]
        private UIItemTemplate m_ItemTemplate = null;

        private Dictionary<int, UIItemTemplate> m_ItemList = null;

        private UnityAction<int, UIItemTemplate> m_OnInitItem, m_OnShowItem;

        private void Awake()
        {
            if (m_ItemTemplate == null)
            {
                Log.Warning("UIItemTemplate is null!!!");
            }

            m_ItemList = new Dictionary<int, UIItemTemplate>();
        }

        public void InitList(int count = 0, int startIndex = 0)
        {
            RemoveAllItems();
            AddItems(count, startIndex);
        }

        public void AddItem(int index)
        {
            var item = Instantiate(m_ItemTemplate, transform);
            item.Init();
            item.name = string.Format("Item_{0}", index);

            m_ItemList.Add(index, item);

            if (m_OnInitItem != null)
            {
                m_OnInitItem(index, item);
            }

            if (m_OnShowItem != null)
            {
                m_OnShowItem(index, item);
            }
        }

        public void AddItems(int count, int startIndex = 0)
        {
            for (int i = 0; i < count; ++i)
            {
                AddItem(startIndex + i);
            }
        }

        public UIItemTemplate GetItem(int index)
        {
            return m_ItemList[index];
        }

        public Dictionary<int, UIItemTemplate> GetAllItems()
        {
            return m_ItemList;
        }

        public void RemoveItem(int index)
        {
            if (m_ItemList.TryGetValue(index, out var item))
            {
                Destroy(item.gameObject);
                m_ItemList.Remove(index);
            }
        }

        public void RemoveAllItems()
        {
            foreach (var item in m_ItemList.Values)
            {
                Destroy(item.gameObject);
            }
            m_ItemList.Clear();
        }

        public void RefreshAllItems()
        {
            foreach (var pair in m_ItemList)
            {
                if (m_OnShowItem != null)
                {
                    m_OnShowItem(pair.Key, pair.Value);
                }
            }
        }

        public void AddListener(UnityAction<int, UIItemTemplate> OnInitItem, UnityAction<int, UIItemTemplate> OnShowItem = null)
        {
            m_OnInitItem += OnInitItem;
            m_OnShowItem += OnShowItem;
        }
    }
}
