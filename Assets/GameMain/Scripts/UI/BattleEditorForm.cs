using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class BattleEditorForm : UIForm
    {
        private ProcedureBattleEditor m_Owner = null;

        private RectTransform m_DrawPanel = null;
        private Toggle m_Paint = null;
        private Toggle m_Erase = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_DrawPanel = GetChild<RectTransform>("m_DrawPanel");
            m_Paint = GetChild<Toggle>("m_Paint");
            m_Erase = GetChild<Toggle>("m_Erase");
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_Owner = userData as ProcedureBattleEditor;
        }
    }
}
