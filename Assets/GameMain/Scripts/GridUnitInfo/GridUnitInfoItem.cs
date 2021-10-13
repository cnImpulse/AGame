using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class GridUnitInfoItem : UIItemTemplate
    {
        private RectTransform m_InfoList = null;
        private Text m_NameText = null;
        private Text m_HpText = null;
        private Text m_MpText = null;

        private Canvas m_ParentCanvas = null;
        private GridUnit m_Owner = null;

        public Entity Owner => m_Owner;

        private void Awake()
        {
            base.Init();

            m_InfoList = GetChild<RectTransform>("m_InfoList");
            m_NameText = GetChild<Text>("m_NameText");
            m_HpText = GetChild<Text>("m_HpText");
            m_MpText = GetChild<Text>("m_MpText");
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

            m_NameText.text = owner.Data.Name;
        }

        public bool Refresh()
        {
            if (m_Owner == null || !m_Owner.Available)
            {
                return false;
            }

            RefreshPos();
            RefreshAttr();

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

        private void RefreshAttr()
        {
            m_HpText.text = string.Format("HP: {0} / {1}", m_Owner.Data.MaxHP, m_Owner.Data.HP);
            if (m_Owner.Data.GridUnitType == GridUnitType.BattleUnit)
            {
                var battleUnit = m_Owner as BattleUnit;
                //m_MpText.text = string.Format("MP: {0} / {1}", battleUnit.Data.MaxMP, battleUnit.Data.MP);
            }
        }

        public void Reset()
        {
            m_Owner = null;
            gameObject.SetActive(false);
        }
    }
}
