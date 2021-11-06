using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace SSRPG
{
    public class SkillActionForm : UIForm
    {
        private Button m_Mask = null;
        private UIListTemplate m_ActionList = null;

        private SkillState m_Owner = null;
        private List<int> m_SkillIdList = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_Mask = GetChild<Button>("m_Mask");
            m_Mask.onClick.AddListener(OnClickMask);
            m_ActionList = GetChild<UIListTemplate>("m_ActionList");
            m_ActionList.AddListener(OnActionItemInit);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_Owner = userData as SkillState;

            m_SkillIdList = m_Owner.Owner.Data.SkillList;
            m_ActionList.InitList(m_SkillIdList.Count);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnClickMask()
        {
            m_Owner.ChangeState<ActionState>();
        }

        private void OnActionItemInit(int index, UIItemTemplate item)
        {
            var button = item.GetComponent<Button>();
            var text = item.GetChild<TextMeshProUGUI>("m_Text");

            var cfg = GameEntry.Cfg.Tables.TblBattleUnitSkill.Get(m_SkillIdList[index]);
            text.text = cfg.Name;
            button.onClick.AddListener(() => { m_Owner.ReleaseSkill(cfg.Id); });
        }
    }
}
