using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;

namespace SSRPG
{
    public class BattleForm : UIForm
    {
        private ProcedureBattle m_ProcedureBattle = null;

        private Button m_StartBtn = null;
        private Button m_SetBtn = null;

        public void OnClickStartBtn()
        {
            m_StartBtn.gameObject.SetActive(false);
            m_ProcedureBattle.StartBattle();
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_StartBtn = GetChild<Button>("m_StartBtn");
            m_SetBtn = GetChild<Button>("m_SetBtn");
            m_StartBtn.onClick.AddListener(OnClickStartBtn);
            m_SetBtn.onClick.AddListener(() => { GameEntry.UI.OpenUIForm(Cfg.UI.FormType.GameSetForm); });
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_ProcedureBattle = userData as ProcedureBattle;

            m_StartBtn.gameObject.SetActive(true);
        }
    }
}
