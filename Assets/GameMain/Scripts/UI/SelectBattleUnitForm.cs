using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class SelectBattleUnitForm : UIForm
    {
        private ProcedureBattle m_ProcedureBattle = null;

        public void OnStartBtnClick()
        {
            m_ProcedureBattle.StartBattle();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_ProcedureBattle = userData as ProcedureBattle;
            if (m_ProcedureBattle == null)
            {
                Log.Warning("ProcedureBattle is invalid when open SelectBattleUnitForm.");
                return;
            }


        }
    }
}
