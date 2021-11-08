using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class BattleState : BattleStateBase
    {
        private IFsm<BattleUnit> m_BattleUnitFsm = null;
        private int m_GridUnitInfoForm = 0;

        protected override void OnInit(IFsm<ProcedureBattle> fsm)
        {
            base.OnInit(fsm);

            GameEntry.Event.Subscribe(EventName.PointerDownGridMap, OnPointGridMap);
        }

        protected override void OnEnter(IFsm<ProcedureBattle> fsm)
        {
            base.OnEnter(fsm);

            GameEntry.Event.Subscribe(EventName.BattleUnitActionCancel, OnBattleUnitActionCancel);
            GameEntry.Event.Subscribe(EventName.BattleUnitActionEnd, OnBattleUnitActionEnd);

            m_BattleUnitFsm = Fsm.GetData<VarObject>("BattleUnitFsm").Value as IFsm<BattleUnit>;
            GameEntry.UI.CloseUIForm(true, m_GridUnitInfoForm);
            m_GridUnitInfoForm = GameEntry.UI.OpenUIForm(Cfg.UI.FormType.GridUnitInfoForm, m_BattleUnitFsm.Owner);
            if (IsAutoBattle)
            {
                m_BattleUnitFsm.Start<AutoActionState>();
            }
            else
            {
                m_BattleUnitFsm.Start<MoveState>();
            }
        }

        protected override void OnUpdate(IFsm<ProcedureBattle> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<ProcedureBattle> fsm, bool isShutdown)
        {
            DestoryBattleUnitFsm();
            GameEntry.UI.CloseUIForm(true, m_GridUnitInfoForm);
            GameEntry.Event.Unsubscribe(EventName.BattleUnitActionCancel, OnBattleUnitActionCancel);
            GameEntry.Event.Unsubscribe(EventName.BattleUnitActionEnd, OnBattleUnitActionEnd);

            base.OnLeave(fsm, isShutdown);
        }

        private void DestoryBattleUnitFsm()
        {
            if (m_BattleUnitFsm == null)
            {
                return;
            }

            GameEntry.Fsm.DestroyFsm(m_BattleUnitFsm);
            Fsm.RemoveData("BattleUnitFsm");
            m_BattleUnitFsm = null;
        }

        private void OnBattleUnitActionEnd(object sender, GameEventArgs e)
        {
            DestoryBattleUnitFsm();
            var battleUnitList = m_GridMap.GetGridUnitList<BattleUnit>(m_ActiveCamp);
            foreach (var battleUnit in battleUnitList)
            {
                if (battleUnit.CanAction)
                {
                    ChangeState<BattleUnitSelectState>();
                    return;
                }
            }
            
            ChangeState<RoundEndState>();
        }

        private void OnBattleUnitActionCancel(object sender, GameEventArgs e)
        {
            DestoryBattleUnitFsm();
            ChangeState<BattleUnitSelectState>();
        }

        private void OnPointGridMap(object sender, GameEventArgs e)
        {
            var ne = e as GameEventBase;
            var gridData = ne.UserData as GridData;
            var gridUnit = gridData.GridUnit;
            if (gridUnit == null)
            {
                return;
            }

            GameEntry.UI.CloseUIForm(true, m_GridUnitInfoForm);
            m_GridUnitInfoForm = GameEntry.UI.OpenUIForm(Cfg.UI.FormType.GridUnitInfoForm, gridUnit);
        }
    }
}