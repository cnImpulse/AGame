using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class BattleForm : UIForm
    {
        private ProcedureBattle m_ProcedureBattle = null;

        private GameObject m_SelectPanel = null;
        private Toggle m_AutoBattleBtn = null;

        public void OnStartBtnClick()
        {
            m_SelectPanel.SetActive(false);
            m_ProcedureBattle.StartBattle();
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_SelectPanel = GetChild("m_SelectPanel").gameObject;
            m_AutoBattleBtn = GetChild<Toggle>("m_AutoBattleBtn");
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_ProcedureBattle = userData as ProcedureBattle;
            if (m_ProcedureBattle == null)
            {
                Log.Warning("ProcedureBattle is invalid when open BattleForm.");
                return;
            }

            m_AutoBattleBtn.isOn = GameEntry.Battle.AutoBattle;
            m_AutoBattleBtn.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(bool isOn)
        {
            GameEntry.Battle.AutoBattle = isOn;
        }
    }
}
