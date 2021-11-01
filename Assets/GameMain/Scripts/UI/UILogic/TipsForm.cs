using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;

namespace SSRPG
{
    public class TipsForm : UIForm
    {
        private Vector3 m_InitialPosition = default;
        private Text m_TipsTxt = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            FadeTime = 1f;
            m_InitialPosition = transform.position;

            m_TipsTxt = GetChild<Text>("m_TipsTxt");
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            m_TipsTxt.DOKill();
            transform.DOKill();
            m_CanvasGroup.DOKill();

            base.OnClose(isShutdown, userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            string tips = userData as string;

            transform.position = m_InitialPosition;
            m_TipsTxt.text = "";
            var tweener = m_TipsTxt.DOText(tips, tips.Length * 0.2f);
            tweener.SetDelay(0.2f);
            tweener.OnComplete(() =>
            {
                var tweener1 = transform.DOMoveY(transform.position.y + 100, FadeTime);
                var tweener2 = m_CanvasGroup.DOFade(0, FadeTime);
                tweener2.OnComplete(() => {
                    Close(true);
                });
            });
        }
    }
}
