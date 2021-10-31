using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class SaveForm : UIForm
    {
        private const int MaxSave = 3;

        private ProcedureMenu m_Owner = null;

        private UIListTemplate m_SaveList = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_SaveList = GetChild<UIListTemplate>("m_SaveList");
            m_SaveList.AddListener(OnSaveListShow);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_Owner = userData as ProcedureMenu;

            m_SaveList.InitList();
            m_SaveList.AddItems(MaxSave);
        }

        private void OnSaveListShow(int index)
        {
            var item = m_SaveList.GetItem(index);
            var m_SaveBtn = item.GetChild<Button>("m_SaveBtn");
            var m_SaveIndex = item.GetChild<Text>("m_SaveIndex");
            var m_SaveTip = item.GetChild<Text>("m_SaveTip");

            m_SaveIndex.text = string.Format("存档{0}: ", index);
            m_SaveTip.text = string.Format("空存档");

            m_SaveBtn.onClick.AddListener(() => { OnClickSaveItem(index); });
        }

        private void OnClickSaveItem(int index)
        {
            m_Owner.SetMenuOption(MenuOption.CreatGame);
        }
    }
}
