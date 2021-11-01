﻿using System.Collections;
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

        private Dropdown m_BattleUnitList = null;

        private Button m_ReturnBtn = null;

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

        public int SelectedBattleUnitId
        {
            get
            {
                return int.Parse(m_BattleUnitList.captionText.text);
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_DrawPanel = GetChild<ToggleGroup>("m_DrawPanel");
            m_Paint = GetChild<Toggle>("m_Paint");
            m_Erase = GetChild<Toggle>("m_Erase");
            m_Fill = GetChild<Toggle>("m_Fill");

            m_BattleUnitList = GetChild<Dropdown>("m_BattleUnitList");

            m_ReturnBtn = GetChild<Button>("m_ReturnBtn");
            m_ReturnBtn.onClick.AddListener(OnClickReturnBtn);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_Owner = userData as ProcedureBattleEditor;

            InitBattleUnitList();
        }

        private void InitBattleUnitList()
        {
            m_BattleUnitList.ClearOptions();
            var battleUnitTbl = GameEntry.Cfg.Tables.TblBattleUnit.DataList;
            List<string> battleUnitList = new List<string>();
            foreach (var cfg in battleUnitTbl)
            {
                battleUnitList.Add(cfg.Id.ToString());
            }
            m_BattleUnitList.AddOptions(battleUnitList);
        }

        private void OnClickReturnBtn()
        {
            m_Owner.ReturnMenuForm();
        }
    }
}