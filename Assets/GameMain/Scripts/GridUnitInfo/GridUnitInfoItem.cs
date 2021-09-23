using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class GridUnitInfoItem : MonoBehaviour
    {
        private RectTransform m_InfoList = null;
        private Text m_NameText = null;
        private Text m_HpText = null;

        private Canvas m_ParentCanvas = null;
        private GridUnit m_Owner = null;

        public Entity Owner
        {
            get
            {
                return m_Owner;
            }
        }

        private void Awake()
        {
            m_InfoList = transform.Find("m_InfoList").GetComponent<RectTransform>();
            m_NameText = m_InfoList.Find("m_NameText").GetComponent<Text>();
            m_HpText   = m_InfoList.Find("m_HpText").GetComponent<Text>();
        }

        public void Init(GridUnit owner, Canvas parentCanvas)
        {
            if (owner == null)
            {
                Log.Error("Owner is invalid.");
                return;
            }

            m_Owner = owner;
            m_ParentCanvas = parentCanvas;
            gameObject.SetActive(true);

            m_NameText.text = owner.GridUnitData.Name;
        }

        public bool Refresh()
        {
            if (m_Owner == null || !m_Owner.Available)
            {
                return false;
            }

            RefreshPos();
            RefreshHp();

            return true;
        }

        private void RefreshPos()
        {
            Vector3 worldPosition = m_Owner.transform.position;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            Vector2 position;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)m_ParentCanvas.transform, screenPosition,
                m_ParentCanvas.worldCamera, out position))
            {
                m_InfoList.localPosition = position;
            }
        }

        private void RefreshHp()
        {
            m_HpText.text = string.Format("{0} / {1}", m_Owner.GridUnitData.MaxHP, m_Owner.GridUnitData.HP);
        }

        public void Reset()
        {
            m_Owner = null;
            gameObject.SetActive(false);
        }
    }
}
