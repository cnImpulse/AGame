using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public enum MenuOption
    {
        None,
        StartGame,
        BattleTest,
        BattleEditor,
    }

    public class MenuForm : UIForm
    {
        private ProcedureMenu m_ProcedureMenu = null;

        private Button m_StartGame = null;
        private Button m_BattleTest = null;
        private Button m_BattleEditor = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_StartGame = GetChild<Button>("m_StartGame");
            m_BattleTest = GetChild<Button>("m_BattleTest");
            m_BattleEditor = GetChild<Button>("m_BattleEditor");

            m_StartGame.onClick.AddListener(OnClickStartGame);
            m_BattleTest.onClick.AddListener(() => OnClickOptionBtn(MenuOption.BattleTest));
            m_BattleEditor.onClick.AddListener(() => OnClickOptionBtn(MenuOption.BattleEditor));
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
            GameEntry.UI.OpenUIForm(UIFormId.SaveForm, m_ProcedureMenu);
        }

        public void OnClickOptionBtn(MenuOption menuOption)
        {
            m_ProcedureMenu.SetMenuOption(menuOption);
        }
    }
}
