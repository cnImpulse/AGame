using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SSRPG
{
    public class UIItemTemplate : MonoBehaviour
    {
        private Dictionary<string, Transform> m_ChildList = null;

        private void Awake()
        {
            InitChildList();
        }

        protected virtual void Init()
        {
            InitChildList();
        }

        private void InitChildList()
        {
            m_ChildList = new Dictionary<string, Transform>();
            var childList = GetComponentsInChildren<Transform>(true);
            Regex regex = new Regex("m_");
            for (int i = 0; i < childList.Length; ++i)
            {
                if (!regex.IsMatch(childList[i].name))
                {
                    continue;
                }

                m_ChildList.Add(childList[i].name, childList[i]);
            }
        }

        /// <summary>
        /// 获取"m_"短命名开头的子节点
        /// </summary>
        public T GetChild<T>(string name)
            where T : Component
        {
            if (m_ChildList.TryGetValue(name, out var child))
            {
                return child.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 获取"m_"短命名开头的子节点
        /// </summary>
        public Transform GetChild(string name)
        {
            if (m_ChildList.TryGetValue(name, out var child))
            {
                return child;
            }
            return null;
        }
    }
}
