using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class ActionForm : UIForm
    {
        private GameObject m_ActionList = null;
        private Button m_AttackBtn = null;
        private Button m_SkillBtn = null;
        private Button m_AwaitBtn = null;

        private UIListTemplate m_SkillList = null;

        private BattleUnitActionState m_ActionState = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_ActionList = transform.Find("m_ActionList").gameObject;
            m_AttackBtn = m_ActionList.transform.Find("m_AttackBtn").GetComponent<Button>();
            m_SkillBtn = m_ActionList.transform.Find("m_SkillBtn").GetComponent<Button>();
            m_AwaitBtn = m_ActionList.transform.Find("m_AwaitBtn").GetComponent<Button>();
            m_SkillList = m_SkillBtn.transform.Find("m_SkillList").GetComponent<UIListTemplate>();

            m_AttackBtn.onClick.AddListener(() => { OnClickBtn(ActionType.Attack); });
            m_AwaitBtn.onClick.AddListener(() => { OnClickBtn(ActionType.Await); });
            m_SkillBtn.onClick.AddListener(OnClickSkillBtn);
            m_SkillList.AddListener(OnShowSkillBtn);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_ActionState = userData as BattleUnitActionState;
            if (m_ActionState == null)
            {
                Log.Warning("BattleUnitActionState is invalid when open ActionForm.");
                return;
            }

            transform.position = Camera.main.WorldToScreenPoint(m_ActionState.ActiveBattleUnit.transform.position);
            m_SkillList.InitList();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }

        private void OnShowSkillBtn(int index)
        {
            var item = m_SkillList.GetItem(index);
            var button = item.GetComponent<Button>();
            var text = item.GetComponentInChildren<Text>();

            var cfg = GameEntry.Cfg.Tables.TblBattleUnitSkill.Get(index);
            item.name = string.Format("SkillBtn_{0}", cfg.Id);
            text.text = cfg.Name;
            button.onClick.AddListener(() => { OnClickSkillOptionBtn(index); });
        }

        private void OnClickBtn(ActionType type)
        {
            m_ActionState.SelectAction(type);
        }

        private void OnClickSkillBtn()
        {
            m_SkillList.RemoveAllItems();
            var skillIdList = m_ActionState.ActiveBattleUnit.Data.SkillList;
            for (int i = 0; i < skillIdList.Count; ++i)
            {
                m_SkillList.AddItem(skillIdList[i]);
            }
        }

        private void OnClickSkillOptionBtn(int skillId)
        {
            Log.Info(skillId);
        }
    }
}
