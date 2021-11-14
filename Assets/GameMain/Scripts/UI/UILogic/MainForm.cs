using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;

namespace SSRPG
{
    public class MainForm : UIForm
    {
        private ProcedureMain m_Owner = null;

        private UIListTemplate m_DiscipleList = null;
        private Dictionary<int, int> m_DiscipleDataList = null;

        private Button m_ExploreBtn = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_DiscipleList = GetChild<UIListTemplate>("m_DiscipleList");
            m_DiscipleList.AddListener(OnInitDiscipleList);

            m_ExploreBtn = GetChild<Button>("m_ExploreBtn");
            m_ExploreBtn.onClick.AddListener(OnClickExploreBtn);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_Owner = userData as ProcedureMain;

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
            var m_HP = item.GetChild<Text>("m_HP");
            var m_MOV = item.GetChild<Text>("m_MOV");
            var m_ATK = item.GetChild<Text>("m_ATK");
            var m_AtkRange = item.GetChild<Text>("m_AtkRange");

            //var cfg = GameEntry.Cfg.Tables.TblRole.Get(index);
            //m_Name.text = cfg.Name;
            //m_Desc.text = cfg.Desc;
            //m_HP.text = string.Format("血量: {0}", cfg.MaxHP);
            //m_MOV.text = string.Format("移动力: {0}", cfg.MOV);
            //m_ATK.text = string.Format("攻击力: {0}", cfg.ATK);
            //m_AtkRange.text = string.Format("攻击范围: {0}", cfg.AtkRange);
        }

        private void OnClickExploreBtn()
        {
            m_Owner.EnterExplore();
        }
    }
}
