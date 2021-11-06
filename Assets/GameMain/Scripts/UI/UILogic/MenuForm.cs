using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public enum MenuOption
    {
        None,
        StartGame,
        BattleTest,
    }

    public class MenuForm : UIForm
    {
        private ProcedureMenu m_ProcedureMenu = null;

        private Button m_StartGame = null;
        private Button m_BattleTest = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_StartGame = GetChild<Button>("m_StartGame");
            m_BattleTest = GetChild<Button>("m_BattleTest");

            m_StartGame.onClick.AddListener(OnClickStartGame);
            m_BattleTest.onClick.AddListener(() => OnClickOptionBtn(MenuOption.BattleTest));
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_ProcedureMenu = userData as ProcedureMenu;
            if (m_ProcedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when open MenuForm.");
                return;
            }
        }

        public void OnClickStartGame()
        {
            GameEntry.UI.OpenUIForm(Cfg.UI.FormType.SaveForm, m_ProcedureMenu);
        }

        public void OnClickOptionBtn(MenuOption menuOption)
        {
            m_ProcedureMenu.SetMenuOption(menuOption);
        }
    }
}
