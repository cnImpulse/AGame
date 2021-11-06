﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public abstract class UIForm : UIFormLogic
    {
        private Dictionary<string, Transform> m_ChildList = null;

        private float m_FadeTime = 0.3f;
        private Canvas m_CachedCanvas = null;
        protected CanvasGroup m_CanvasGroup = null;

        public UnityAction OnCloseForm = null;

        public void Close()
        {
            Close(false);
        }

        public void Close(bool ignoreFade)
        {
            StopAllCoroutines();

            if (ignoreFade)
            {
                GameEntry.UI.CloseUIForm(UIForm);
            }
            else
            {
                StartCoroutine(CloseCo(m_FadeTime));
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

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
            if (m_CloseBtn != null)
            {
                m_CloseBtn.onClick.AddListener(Close);
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
            StopAllCoroutines();
            StartCoroutine(m_CanvasGroup.FadeToAlpha(1f, m_FadeTime));
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            if (OnCloseForm != null)
            {
                OnCloseForm();
            }

            base.OnClose(isShutdown, userData);
        }

        private IEnumerator CloseCo(float duration)
        {
            yield return m_CanvasGroup.FadeToAlpha(0f, duration);
            GameEntry.UI.CloseUIForm(UIForm);
        }
    }
}
