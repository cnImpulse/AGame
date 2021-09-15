using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 玩家回合，选择战斗单位状态
    /// </summary>
    public class SelectBattleUnitState : FsmState<ProcedureBattle>
    {
        private int m_EffectId = 0;
        private BattleUnit m_SelectBattleUnit = null;

        protected override void OnEnter(IFsm<ProcedureBattle> fsm)
        {
            base.OnEnter(fsm);

            Log.Info("进入选择状态。");

            GameEntry.Event.Subscribe(PointGridMapEventArgs.EventId, OnPointGridMap);
        }

        protected override void OnUpdate(IFsm<ProcedureBattle> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            if (m_SelectBattleUnit != null && fsm.Owner.ActiveCamp == m_SelectBattleUnit.BattleUnitData.CampType)
            {
                VarObject data = new VarObject();
                data.Value = m_SelectBattleUnit;

                fsm.SetData("ActiveBattleUnit", data);
                ChangeState<BattleUnitMoveState>(fsm);
            }
        }

        protected override void OnLeave(IFsm<ProcedureBattle> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            UnSelectBattleUnit();

            GameEntry.Event.Unsubscribe(PointGridMapEventArgs.EventId, OnPointGridMap);

            Log.Info("离开选择状态。");
        }

        private void OnPointGridMap(object sender, GameEventArgs e)
        {
            PointGridMapEventArgs ne = (PointGridMapEventArgs)e;
            GridUnit gridUnit = ne.gridData.GridUnit;
            if (gridUnit == null || gridUnit.GridUnitData.GridUnitType != GridUnitType.BattleUnit)
            {
                UnSelectBattleUnit();
                return;
            }

            SelectBattleUnit(gridUnit as BattleUnit);
        }

        private void SelectBattleUnit(BattleUnit battleUnit)
        {
            m_SelectBattleUnit = battleUnit;

            ShowSelectEffect(battleUnit.transform.position);
        }

        private void UnSelectBattleUnit()
        {
            m_SelectBattleUnit = null;
            HideSelectEffect();
        }

        private void ShowSelectEffect(Vector3 position)
        {
            if (m_EffectId == 0)
            {
                m_EffectId = GameEntry.Effect.CreatEffect(EffectType.SelectType, position);
            }
            else
            {
                GameEntry.Effect.ChangeEffectPos(m_EffectId, position);
            }
        }

        private void HideSelectEffect()
        {
            GameEntry.Effect.DestoryEffect(m_EffectId);
            m_EffectId = 0;
        }
    }
}