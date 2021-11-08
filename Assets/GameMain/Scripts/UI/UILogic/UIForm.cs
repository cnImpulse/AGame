using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;

namespace SSRPG
{
    public abstract class UIForm : UIFormLogic
    {
        private Dictionary<string, Transform> m_ChildList = null;

        [SerializeField]
        private bool m_FadeOpen = true;
        [SerializeField]
        private float m_FadeTime = 0.3f;
        private Canvas m_CachedCanvas = null;
        protected CanvasGroup m_CanvasGroup = null;

        public void Close(bool ignoreFade = false)
        {
            if (ignoreFade)
            {
                GameEntry.UI.CloseUIForm(UIForm);
            }
            else
            {
                var tweener = m_CanvasGroup.DOFade(0, m_FadeTime);
                tweener.OnComplete(() => { GameEntry.UI.CloseUIForm(UIForm); });
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            gameObject.SetLayerRecursively(LayerMask.NameToLayer("UI"));

            m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();

            RectTransform transform = GetComponent<RectTransform>();
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;

            gameObject.GetOrAddComponent<GraphicRaycaster>();

            InitChildList();

            var m_CloseBtn = GetChild<Button>("m_CloseBtn");
            var m_CloseMask = GetChild<Button>("m_CloseMask");
            if (m_CloseBtn != null)
            {
                m_CloseBtn.onClick.AddListener(() => { Close(); });
            }
            if (m_CloseMask != null)
            {
                m_CloseMask.onClick.AddListener(() => { Close(); });
            }
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

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_CanvasGroup.alpha = 0f;
            if (m_FadeOpen)
            {
                m_CanvasGroup.DOFade(1, m_FadeTime);
            }
            else
            {
                m_CanvasGroup.alpha = 1f;
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            m_CanvasGroup.DOKill();

            base.OnClose(isShutdown, userData);
        }
    }
}
