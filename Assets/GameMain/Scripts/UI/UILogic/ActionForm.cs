﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SSRPG
{
    public class ActionForm : UIForm
    {
        private Button m_Mask = null;
        private UIListTemplate m_ActionList = null;

        private ActionState m_Owner = null;

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

            m_Owner = userData as ActionState;

            InitActionListPos();
            m_ActionList.InitList();
            m_ActionList.AddItem((int)ActionType.Skill);
            m_ActionList.AddItem((int)ActionType.Await);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void InitActionListPos()
        {
            var isLeft = Camera.main.WorldToViewportPoint(m_Owner.Owner.transform.position).x < 0.5;
            m_ActionList.transform.SetLocalPositionX(isLeft ? (Screen.width - 610) : 100);
        }

        private void OnClickMask()
        {
            Close();
            GameEntry.Event.Fire(this, EventName.BattleUnitActionCancel);
        }

        private void OnActionItemInit(int index, UIItemTemplate item)
        {
            var button = item.GetComponent<Button>();
            var text = item.GetChild<TextMeshProUGUI>("m_Text");

            var type = (ActionType)index;
            if (type == ActionType.Skill)
            {
                text.text = "技能";
                if (m_Owner.Owner.Data.SkillList.Count == 0)
                {
                    button.interactable = false;
                }
            }
            else if (type == ActionType.Await)
            {
                text.text = "待机";
            }
            else
            {
                text.text = "Error!";
            }

            button.onClick.AddListener(() => { Action(type); });
        }

        private void Action(ActionType type)
        {
            m_Owner.SelectAction(type);
        }
    }
}
