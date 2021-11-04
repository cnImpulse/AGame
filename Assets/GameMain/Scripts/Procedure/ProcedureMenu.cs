﻿using GameFramework.Event;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace SSRPG
{
    public class ProcedureMenu : ProcedureBase
    {
        private MenuOption m_MenuOption = MenuOption.None;
        private MenuForm m_MenuForm = null;
        private int m_MenuSerialId = 0;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("菜单界面。");

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            m_MenuSerialId = (int)GameEntry.UI.OpenUIForm(UIFormId.MenuForm, this);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_MenuOption == MenuOption.None)
            {
                return;
            }

            SceneType sceneType = SceneType.None;
            if (m_MenuOption == MenuOption.StartGame)
            {
                sceneType = SceneType.Main;
            }
            else if (m_MenuOption == MenuOption.BattleTest)
            {
                sceneType = SceneType.Battle;
            }

            procedureOwner.SetData<VarInt32>("NextSceneId", (int)sceneType);
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            m_MenuOption = MenuOption.None;
            if (m_MenuForm != null)
            {
                m_MenuForm.Close(isShutdown);
                m_MenuForm = null;
            }

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
        }

        public void SetMenuOption(MenuOption menuOption)
        {
            m_MenuOption = menuOption;
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UIForm.SerialId != m_MenuSerialId)
            {
                return;
            }

            m_MenuForm = (MenuForm)ne.UIForm.Logic;
        }
    }
}
