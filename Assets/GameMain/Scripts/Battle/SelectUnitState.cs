using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 玩家回合，选择单位行动状态
    /// </summary>
    public class SelectUnitState : FsmState<ProcedureBattle>
    {
        private BattleUnit selectUnit = null;
        private GridMap gridMap = null;
        private int effectId = 0;

        protected override void OnInit(IFsm<ProcedureBattle> fsm)
        {
            base.OnInit(fsm);
            Log.Info("创建选择状态。");
        }

        protected override void OnEnter(IFsm<ProcedureBattle> fsm)
        {
            base.OnEnter(fsm);

            GameEntry.Event.Subscribe(PointGridMapEventArgs.EventId, OnPointGridMap);

            gridMap = GameEntry.Entity.GetEntity(fsm.Owner.GridMapData.Id).Logic as GridMap;

            Log.Info("进入选择状态。");
        }

        protected override void OnLeave(IFsm<ProcedureBattle> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            GameEntry.Event.Unsubscribe(PointGridMapEventArgs.EventId, OnPointGridMap);

            Log.Info("离开选择状态。");
        }

        protected override void OnDestroy(IFsm<ProcedureBattle> fsm)
        {
            base.OnDestroy(fsm);
            Log.Info("销毁选择状态。");
        }

        private void OnPointGridMap(object sender, GameEventArgs e)
        {
            PointGridMapEventArgs ne = (PointGridMapEventArgs)e;
            GridUnit gridUnit = ne.gridData.GridUnit;
            if (gridUnit == null || gridUnit.GridData.GridUnitType != GridUnitType.BattleUnit)
            {
                UnSelectBattleUnit();
                return;
            }

            SelectBattleUnit(gridUnit as BattleUnit);
        }

        private void SelectBattleUnit(BattleUnit battleUnit)
        {
            selectUnit = battleUnit;
            ShowSelectEffect(selectUnit.transform.position);

            if (battleUnit.BattleUnitData.CampType == CampType.Player)
            {
                var canMoveList = gridMap.GridMapData.GetCanMoveGrids(battleUnit.BattleUnitData);
                gridMap.ShowCanMoveArea(canMoveList);
            }
        }

        private void UnSelectBattleUnit()
        {
            HideSelectEffect();
            selectUnit = null;
        }

        private void ShowSelectEffect(Vector3 position)
        {
            if (effectId == 0)
            {
                effectId = GameEntry.Effect.CreatEffect(EffectType.SelectType, position);
            }
            else
            {
                GameEntry.Effect.ChangeEffectPos(effectId, position);
            }
        }

        private void HideSelectEffect()
        {
            GameEntry.Effect.DestoryEffect(effectId);
            effectId = 0;
        }
    }
}