using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;
using TMPro;

namespace SSRPG
{
    public class TextBubbleForm : UIForm
    {
        private TextMeshProUGUI m_Txt = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_Txt = GetChild<TextMeshProUGUI>("m_Txt");
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            var info = userData as DamageInfo;

            Vector2 position;
            var worldPos = GameEntry.Entity.GetEntity(info.TargetId).transform.position;
            var screenPos = Camera.main.WorldToScreenPoint(worldPos);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)m_CachedCanvas.transform, screenPos,
                m_CachedCanvas.worldCamera, out position))
            {
                m_Txt.transform.localPosition = position;
            }
            else
            {
                Close(true);
                return;
            }

            m_CanvasGroup.alpha = 1;
            m_Txt.text = "-" + info.DamageHP;
            var tweener = m_Txt.transform.DOMoveY(m_Txt.transform.position.y + 100, 1);
            var tweener1 = m_CanvasGroup.DOFade(0, 1);
            tweener1.SetDelay(1f);
            tweener1.OnComplete(() =>
            {
                Close(true);
            });
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            m_Txt.DOKill();
            m_Txt.transform.DOKill();
            m_CanvasGroup.DOKill();

            base.OnClose(isShutdown, userData);
        }
    }
}
