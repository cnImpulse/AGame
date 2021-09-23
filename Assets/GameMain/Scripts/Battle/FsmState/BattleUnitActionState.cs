using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 玩家回合，选择战斗单位行动指令状态
    /// </summary>
    public class BattleUnitActionState : FsmState<ProcedureBattle>
    {
        private GridMap m_GridMap = null;

        private ActionForm m_Form = null;
        private ActionType m_ActionType = ActionType.None;
        private BattleUnit m_ActiveBattleUnit = null;

        public BattleUnit ActiveBattleUnit => m_ActiveBattleUnit;

        protected override void OnEnter(IFsm<ProcedureBattle> fsm)
        {
            base.OnEnter(fsm);

            Log.Info("进入行动指令选择状态。");

            GameEntry.Event.Subscribe(PointGridMapEventArgs.EventId, OnPointGridMap);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            if (m_GridMap == null)
            {
                m_GridMap = fsm.Owner.gridMap;
            }

            m_ActiveBattleUnit = fsm.GetData("ActiveBattleUnit").GetValue() as BattleUnit;
            GameEntry.UI.OpenUIForm(UIFormId.ActionForm, this);
        }

        protected override void OnUpdate(IFsm<ProcedureBattle> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            if (m_ActiveBattleUnit == null)
            {
                ChangeState<SelectBattleUnitState>(fsm);
            }

            if (m_ActionType == ActionType.Attack)
            {
                ChangeState<BattleUnitAttackState>(fsm);
            }
            else if (m_ActionType == ActionType.Await)
            {
                ChangeState<BattleUnitEndActionState>(fsm);
            }
        }

        protected override void OnLeave(IFsm<ProcedureBattle> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            m_ActionType = ActionType.None;
            m_ActiveBattleUnit = null;

            if (m_Form != null)
            {
                m_Form.Close(isShutdown);
                m_Form = null;
            }

            GameEntry.Event.Unsubscribe(PointGridMapEventArgs.EventId, OnPointGridMap);
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            Log.Info("进入行动指令选择状态。");
        }

        public void SelectAction(ActionType actionType)
        {
            m_ActionType = actionType;
        }

        private void OnPointGridMap(object sender, GameEventArgs e)
        {
            PointGridMapEventArgs ne = (PointGridMapEventArgs)e;

            // tocode
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_Form = (ActionForm)ne.UIForm.Logic;
        }
    }
}