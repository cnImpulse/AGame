using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class BattleEndForm : UIForm
    {
        private ProcedureBattleEnd m_ProcedureBattleEnd = null;

        private Button m_AgainBtn = null;
        private Text m_ResultText = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_AgainBtn = transform.Find("Panel/m_AgainBtn").GetComponent<Button>();
            m_ResultText = transform.Find("Panel/m_ResultText").GetComponent<Text>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_ProcedureBattleEnd = userData as ProcedureBattleEnd;
            if (m_ProcedureBattleEnd == null)
            {
                Log.Warning("ProcedureBattleEnd is invalid when open BattleEndForm.");
                return;
            }

            ShowBattleResult();
            m_AgainBtn.onClick.AddListener(OnClickBtn);
        }

        private void ShowBattleResult()
        {
            BattleResultInfo info = GameEntry.DataNode.GetData<BattleResultInfo>("BattleResultInfo");
            if (info.WinCampType == CampType.Player)
            {
                m_ResultText.text = string.Format("胜利");
            }
            else
            {
                m_ResultText.text = string.Format("失败");
            }
        }

        private void OnClickBtn()
        {
            m_ProcedureBattleEnd.AgainBattle();
        }
    }
}
