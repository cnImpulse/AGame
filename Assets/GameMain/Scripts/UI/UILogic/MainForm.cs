using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;

namespace SSRPG
{
    public class MainForm : UIForm
    {
        private UIListTemplate m_DiscipleList = null;
        private Dictionary<int, int> m_DiscipleDataList = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_DiscipleList = GetChild<UIListTemplate>("m_DiscipleList");
            m_DiscipleList.AddListener(OnInitDiscipleList);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_DiscipleList.InitList();

            m_DiscipleDataList = GameEntry.Save.SaveData.DiscipleList;
            foreach (var key in m_DiscipleDataList.Keys)
            {
                m_DiscipleList.AddItem(key);
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {

            base.OnClose(isShutdown, userData);
        }

        private void OnInitDiscipleList(int index, UIItemTemplate item)
        {
            var m_Name = item.GetChild<Text>("m_Name");
            var m_Desc = item.GetChild<Text>("m_Desc");

            var cfg = GameEntry.Cfg.Tables.TblBattleUnit.Get(index);
            m_Name.text = cfg.Name;
            m_Desc.text = cfg.Desc;
        }
    }
}
