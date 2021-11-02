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
            m_SaveList.AddListener(OnSaveListInit, OnSaveListShow);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_Owner = userData as ProcedureMenu;

            m_SaveList.InitList();
            m_SaveList.AddItems(MaxSave);
        }

        private void OnSaveListInit(int index)
        {
            var item = m_SaveList.GetItem(index);
            var m_SaveBtn = item.GetChild<Button>("m_SaveBtn");
            var m_SaveIndex = item.GetChild<Text>("m_SaveIndex");
            var m_SaveTip = item.GetChild<Text>("m_SaveTip");
            var m_DeleteBtn = item.GetChild<Button>("m_DeleteBtn");

            m_SaveIndex.text = string.Format("存档{0}: ", index);
            if (GameEntry.Save.HasSave(index))
            {
                var data = GameEntry.Save.GetSaveData(index);
                m_SaveTip.text = data.SaveName;
                m_DeleteBtn.onClick.AddListener(() => { OnClickDeleteBtn(index); });
            }
            else
            {
                m_SaveTip.text = "空存档";
            }

            m_SaveBtn.onClick.AddListener(() => { OnClickSaveItem(index); });
        }

        private void OnSaveListShow(int index)
        {
            var item = m_SaveList.GetItem(index);
            var m_SaveTip = item.GetChild<Text>("m_SaveTip");
            if (GameEntry.Save.HasSave(index))
            {
                var data = GameEntry.Save.GetSaveData(index);
                m_SaveTip.text = data.SaveName;
            }
            else
            {
                m_SaveTip.text = "空存档";
            }
        }

        private void OnClickSaveItem(int index)
        {
            GameEntry.DataNode.SetData("SaveIndex", (VarInt32)index);
            m_Owner.SetMenuOption(MenuOption.StartGame);
        }

        private void OnClickDeleteBtn(int index)
        {
            GameEntry.Save.Delete(index);
            m_SaveList.RefreshAllItems();
        }
    }
}
