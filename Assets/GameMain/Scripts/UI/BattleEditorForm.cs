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

        private ToggleGroup m_DrawPanel = null;
        private Toggle m_Paint = null;
        private Toggle m_Erase = null;
        private Toggle m_Fill = null;

        public EditMode EditMode
        {
            get
            {
                if (m_Paint.isOn)
                {
                    return EditMode.Paint;
                }
                else if (m_Erase.isOn)
                {
                    return EditMode.Erase;
                }
                else if (m_Fill.isOn)
                {
                    return EditMode.Fill;
                }
                else
                {
                    return EditMode.None;
                }
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_DrawPanel = GetChild<ToggleGroup>("m_DrawPanel");
            m_Paint = GetChild<Toggle>("m_Paint");
            m_Erase = GetChild<Toggle>("m_Erase");
            m_Fill = GetChild<Toggle>("m_Fill");
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_Owner = userData as ProcedureBattleEditor;
        }
    }
}
