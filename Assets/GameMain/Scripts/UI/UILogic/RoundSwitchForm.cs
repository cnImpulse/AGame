using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;
using TMPro;

namespace SSRPG
{
    public class RoundSwitchForm : UIForm
    {
        private RoundStartState m_Owner = null;

        private Vector3 m_InitialPosition = default;
        private RectTransform m_Banner = null;
        private TextMeshProUGUI m_BannerTxt = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_Banner = GetChild<RectTransform>("m_Banner");
            m_BannerTxt = GetChild<TextMeshProUGUI>("m_BannerTxt");
            m_InitialPosition = m_Banner.transform.position;
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_Owner = userData as RoundStartState;

            m_CanvasGroup.alpha = 0;
            m_Banner.transform.localScale = Vector3.one;
            m_Banner.transform.position = m_InitialPosition;

            if (m_Owner.m_ActiveCamp == CampType.Player)
            {
                m_BannerTxt.text = "玩家回合";
            }
            else
            {
                m_BannerTxt.text = "敌方回合";
            }
            
            var tweener = m_CanvasGroup.DOFade(0.8f, 1.5f);
            tweener.OnComplete(() =>
            {
                //m_Banner.DOMoveY(1080, 2f);
                //m_Banner.DOScale(0, 2f);
                var tweener = m_CanvasGroup.DOFade(0, 1.5f);
                tweener.SetDelay(0.5f);
                tweener.OnComplete(() =>
                {
                    m_Owner.ChangeState<BattleUnitSelectState>();
                    Close(true);
                });
            });
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            m_Banner.DOKill();
            m_CanvasGroup.DOKill();

            base.OnClose(isShutdown, userData);
        }
    }
}
