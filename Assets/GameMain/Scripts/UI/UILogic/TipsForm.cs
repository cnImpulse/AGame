using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;
using TMPro;

namespace SSRPG
{
    public class TipsForm : UIForm
    {
        private Vector3 m_InitialPosition = default;
        private TextMeshProUGUI m_TipsTxt = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_InitialPosition = transform.position;

            m_TipsTxt = GetChild<TextMeshProUGUI>("m_TipsTxt");
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_TipsTxt.text = userData as string;

            transform.position = m_InitialPosition;

            var tweener = transform.DOMoveY(transform.position.y + 100, 1);
            tweener.OnComplete(() =>
            {
                transform.DOMoveY(transform.position.y + 100, 1);
                var tweener2 = m_CanvasGroup.DOFade(0, 1);
                tweener2.OnComplete(() =>
                {
                    Close(true);
                });
            });
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            m_TipsTxt.DOKill();
            transform.DOKill();
            m_CanvasGroup.DOKill();

            base.OnClose(isShutdown, userData);
        }
    }
}
