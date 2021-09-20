using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class HPBarItem : MonoBehaviour
    {
        private Text m_HpText = null;

        private Canvas m_ParentCanvas = null;
        private RectTransform m_CachedTransform = null;
        private CanvasGroup m_CachedCanvasGroup = null;
        private Entity m_Owner = null;
        private int m_OwnerId = 0;

        public Entity Owner
        {
            get
            {
                return m_Owner;
            }
        }

        private void Awake()
        {
            m_HpText = GetComponentInChildren<Text>();

            m_CachedTransform = m_HpText.GetComponent<RectTransform>();
            m_CachedCanvasGroup = GetComponent<CanvasGroup>();
        }

        public void Init(Entity owner, Canvas parentCanvas, int hp, int maxHp)
        {
            if (owner == null)
            {
                Log.Error("Owner is invalid.");
                return;
            }

            m_ParentCanvas = parentCanvas;
            gameObject.SetActive(true);
            StopAllCoroutines();

            m_CachedCanvasGroup.alpha = 1f;
            if (m_Owner != owner || m_OwnerId != owner.Id)
            {
                m_Owner = owner;
                m_OwnerId = owner.Id;
            }

            m_HpText.text = string.Format("{0} / {1}", maxHp, hp);
            Refresh();

            StartCoroutine(HPBarCo(1.5f));
        }

        public bool Refresh()
        {
            if (m_CachedCanvasGroup.alpha <= 0f)
            {
                return false;
            }

            if (m_Owner != null && Owner.Available && Owner.Id == m_OwnerId)
            {
                Vector3 worldPosition = m_Owner.transform.position;
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

                Vector2 position;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)m_ParentCanvas.transform, screenPosition,
                    m_ParentCanvas.worldCamera, out position))
                {
                    m_CachedTransform.localPosition = position;
                }
            }

            return true;
        }

        public void Reset()
        {
            StopAllCoroutines();
            m_Owner = null;
            m_CachedCanvasGroup.alpha = 1f;
            gameObject.SetActive(false);
        }

        private IEnumerator HPBarCo(float fadeOutDuration)
        {
            yield return m_CachedCanvasGroup.FadeToAlpha(0f, fadeOutDuration);
        }
    }
}
