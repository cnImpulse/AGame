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

        private BattleUnitActionState m_ActionState = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_ActionList = transform.Find("m_ActionList").gameObject;
            m_AttackBtn = transform.Find("m_ActionList/m_AttackBtn").GetComponent<Button>();
            m_SkillBtn = transform.Find("m_ActionList/m_SkillBtn").GetComponent<Button>();
            m_AwaitBtn = transform.Find("m_ActionList/m_AwaitBtn").GetComponent<Button>();
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
            m_AttackBtn.onClick.AddListener(delegate () { OnClickBtn(ActionType.Attack); });
            m_AwaitBtn.onClick.AddListener(delegate () { OnClickBtn(ActionType.Await); });
        }

        private void OnClickBtn(ActionType type)
        {
            m_ActionState.SelectAction(type);
        }
    }
}
