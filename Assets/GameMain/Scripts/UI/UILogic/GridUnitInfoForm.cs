using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;
using TMPro;

namespace SSRPG
{
    public class GridUnitInfoForm : UIForm
    {
        private GridUnit m_Owner = null;

        private Image m_Img = null;
        private TextMeshProUGUI m_NameTxt = null;
        private TextMeshProUGUI m_HpTxt = null;
        private TextMeshProUGUI m_MpTxt = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_Img = GetChild<Image>("m_Img");
            m_NameTxt = GetChild<TextMeshProUGUI>("m_NameTxt");
            m_HpTxt = GetChild<TextMeshProUGUI>("m_HpTxt");
            m_MpTxt = GetChild<TextMeshProUGUI>("m_MpTxt");
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_Owner = userData as GridUnit;

            var render = m_Owner.GetComponent<SpriteRenderer>();
            m_Img.sprite = render.sprite;
            m_Img.color = render.color;
            m_NameTxt.text = m_Owner.Data.Name;
            m_HpTxt.text = string.Format("HP:{0}/{1}", m_Owner.Data.MaxHP, m_Owner.Data.HP);
            if (m_Owner is BattleUnit)
            {
                var battleUnit = m_Owner as BattleUnit;
                m_MpTxt.text = string.Format("MP:{0}/{1}", battleUnit.Data.MaxMP, battleUnit.Data.MP);
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}
