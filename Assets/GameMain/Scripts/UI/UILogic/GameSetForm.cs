using UnityEngine.UI;
using UnityGameFramework.Runtime;
using TMPro;

namespace SSRPG
{
    public class GameSetForm : UIForm
    {
        private UIListTemplate m_SettingList = null;
        private Toggle m_AutoBattle = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_SettingList = GetChild<UIListTemplate>("m_SettingList");
            m_AutoBattle = GetChild<Toggle>("m_AutoBattle");
            m_SettingList.AddListener(OnSettingItemInit);
            m_AutoBattle.onValueChanged.AddListener((isOn) => { GameEntry.Battle.AutoBattle = isOn; });
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            //m_AutoBattle.isOn = GameEntry.Battle.AutoBattle;
        }

        private void OnSettingItemInit(int index, UIItemTemplate item)
        {
            
        }
    }
}
