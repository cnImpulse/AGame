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

        protected override void OnInit(IFsm<ProcedureBattle> fsm)
        {
            base.OnInit(fsm);
            Log.Info("创建选择状态。");
        }

        protected override void OnEnter(IFsm<ProcedureBattle> fsm)
        {
            base.OnEnter(fsm);

            GameEntry.Event.Subscribe(PointGridMapEventArgs.EventId, OnPointGridMap);

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
            if (gridUnit == null || gridUnit.Data.GridUnitType != GridUnitType.BattleUnit)
            {
                return;
            }

            BattleUnit battleUnit = gridUnit as BattleUnit;
            Log.Info(battleUnit.Name);
            GameEntry.Effect.CreatEffect(EffectType.SelectType, battleUnit.transform.position);
        }
    }
}