using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public enum MenuOption
    {
        None,
        StartGame,
    }

    public class MenuForm : UIForm
    {
        private ProcedureMenu m_Owner = null;

        private Button m_StartBtn = null;
        private Button m_SetBtn = null;
        private Button m_ExitBtn = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_StartBtn = GetChild<Button>("m_StartBtn");
            m_SetBtn = GetChild<Button>("m_SetBtn");
            m_ExitBtn = GetChild<Button>("m_ExitBtn");

            m_StartBtn.onClick.AddListener(OnClickStartBtn);
            m_SetBtn.onClick.AddListener(OnClickSetBtn);
            m_ExitBtn.onClick.AddListener(OnClickExitBtn);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_Owner = userData as ProcedureMenu;
            if (m_Owner == null)
            {
                Log.Warning("ProcedureMenu is invalid when open MenuForm.");
                return;
            }
        }

        public void OnClickStartBtn()
        {
            GameEntry.UI.OpenUIForm(Cfg.UI.FormType.SaveForm, m_Owner);
        }

        public void OnClickSetBtn()
        {
            GameEntry.UI.OpenUIForm(Cfg.UI.FormType.GameSetForm, m_Owner);
        }

        public void OnClickExitBtn()
        {
            Application.Quit();
        }
    }
}
