﻿using UnityEngine;
using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace SSRPG
{
    public class ProcedureBattle : ProcedureBase
    {
        private bool m_BattleEnd = false;
        private LevelData m_BattleData = null;
        private BattleForm m_BattleForm = null;

        private IFsm<ProcedureBattle> m_BattleFsm = null;

        public GridMap gridMap = null;

        public void StartBattle()
        {
            Log.Info("战斗开始。");

            GameEntry.Battle.InitBattle(gridMap);
            m_BattleFsm.Start<RoundSwitchState>();
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("进入战斗准备阶段。");

            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowGirdMapSuccess);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(GridUnitDeadEventArgs.EventId, OnGridUnitDead);

            InitBattleFsm();
            InitBattle(GameEntry.Battle.BattleId);
            InitBattleUnitSelect();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_BattleEnd)
            {
                ChangeState<ProcedureBattleEnd>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Entity.HideEntity(gridMap);
            GameEntry.Fsm.DestroyFsm(m_BattleFsm);
            
            gridMap = null;
            m_BattleEnd = false;
            m_BattleData = null;

            if (m_BattleForm != null)
            {
                m_BattleForm.Close(isShutdown);
                m_BattleForm = null;
            }

            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowGirdMapSuccess);
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Unsubscribe(GridUnitDeadEventArgs.EventId, OnGridUnitDead);
        }

        private void InitBattleFsm()
        {
            m_BattleFsm = GameEntry.Fsm.CreateFsm(this, new RoundSwitchState(), new PlayerSelectState(),
                new BattleUnitMoveState(), new PlayerActionState(), new BattleUnitAttackState(),
                new SkillReleaseState(), new AutoActionState(), new BattleUnitEndActionState());
        }

        private void InitBattle(int levelId)
        {
            string path = AssetUtl.GetLevelData(levelId);
            GameEntry.Resource.LoadAsset(path, (assetName, asset, duration, userData) =>
            {
                TextAsset textAsset = asset as TextAsset;
                m_BattleData = Utility.Json.ToObject<LevelData>(textAsset.text);
                GameEntry.Entity.ShowGridMap(0);
            });
        }

        private void InitBattleUnit()
        {
            // 加载敌人
            foreach (var enemy in m_BattleData.enemyList)
            {
                int typeId = enemy.Value + 1;
                var gridData = gridMap.Data.GetGridData(enemy.Key);
                BattleUnitData battleUnitData = new BattleUnitData(typeId, gridData.GridPos, CampType.Enemy);
                gridMap.RegisterBattleUnit(battleUnitData);
            }

            // 加载玩家战棋
            int posCount = m_BattleData.playerBrithList.Count;
            foreach(var position in m_BattleData.playerBrithList)
            {
                int typeId = 10000 + Random.Range(1, 6);
                BattleUnitData battleUnitData = new BattleUnitData(typeId, position, CampType.Player);
                gridMap.RegisterBattleUnit(battleUnitData);
            }
        }

        private void InitBattleUnitSelect()
        {
            GameEntry.UI.OpenUIForm(UIFormId.BattleForm, this);
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_BattleForm = (BattleForm)ne.UIForm.Logic;
        }

        private void OnShowGirdMapSuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType == typeof(GridMap))
            {
                gridMap = ne.Entity.Logic as GridMap;
                InitBattleUnit();
            }
        }

        private void OnGridUnitDead(object sender, GameEventArgs e)
        {
            GridUnitDeadEventArgs ne = (GridUnitDeadEventArgs)e;

            GridUnitData data = ne.gridUnit.Data;
            gridMap.UnRegisterGridUnit(ne.gridUnit);
            if (data.GridUnitType == GridUnitType.BattleUnit)
            {
                var battleUnitList = gridMap.GetBattleUnitList(data.CampType);
                if (battleUnitList.Count == 0)
                {
                    m_BattleEnd = true;
                    BattleResultInfo info = null;
                    if (data.CampType == CampType.Player)
                    {
                        info = new BattleResultInfo(CampType.Enemy);
                    }
                    else if(data.CampType == CampType.Enemy)
                    {
                        info = new BattleResultInfo(CampType.Player);
                    }
                    GameEntry.DataNode.SetData("BattleResultInfo", info);
                    GameEntry.Fsm.DestroyFsm(m_BattleFsm);
                }
            }
        }
    }
}
