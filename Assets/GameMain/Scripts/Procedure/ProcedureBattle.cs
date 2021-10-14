﻿using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Event;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace SSRPG
{
    public class ProcedureBattle : ProcedureBase
    {
        private BattleData m_BattleData = null;
        private bool m_BattleEnd = false;
        private BattleForm m_BattleForm = null;

        private IFsm<ProcedureBattle> m_BattleFsm = null;

        public GridMap gridMap;
        public CampType activeCamp = CampType.None;

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

            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(GridUnitDeadEventArgs.EventId, OnGridUnitDead);

            Log.Info("进入战斗准备阶段。");

            InitBattleFsm();
            InitBattle();
            SelectPlayerBattleUnit();
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
            activeCamp = default;
            m_BattleEnd = false;
            m_BattleData = null;

            if (m_BattleForm != null)
            {
                m_BattleForm.Close(isShutdown);
                m_BattleForm = null;
            }

            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Unsubscribe(GridUnitDeadEventArgs.EventId, OnGridUnitDead);
        }

        private void InitBattleFsm()
        {
            m_BattleFsm = GameEntry.Fsm.CreateFsm(this, new RoundSwitchState(), new PlayerSelectState(),
                new BattleUnitMoveState(), new PlayerActionState(), new BattleUnitAttackState(),
                new SkillReleaseState(), new AutoActionState(), new BattleUnitEndActionState());
        }

        private void InitBattle()
        {
            m_BattleEnd = false;

            int battleId = 2;
            string path = AssetUtl.GetBattleDataPath(battleId);
            GameEntry.Resource.LoadAsset(path, new LoadAssetCallbacks(OnLoadAssetSuccess));
        }

        private void OnLoadAssetSuccess(string assetName, object asset, float duration, object userData)
        {
            TextAsset textAsset = asset as TextAsset;
            m_BattleData = Newtonsoft.Json.JsonConvert.DeserializeObject<BattleData>(textAsset.text);

            GameEntry.Entity.ShowGridMap(m_BattleData.mapId);
        }

        private void InitBattleUnit(int mapEntityId)
        {
            // 加载敌人
            for (int i = 0; i < m_BattleData.enemyIds.Count; ++i)
            {
                int typeId = m_BattleData.enemyIds[i] + Random.Range(1, 6);
                Vector2Int pos = m_BattleData.enemyPos[i];

                BattleUnitData battleUnitData = new BattleUnitData(typeId, mapEntityId, pos, CampType.Enemy);
                GameEntry.Entity.ShowBattleUnit(battleUnitData);
            }

            // 加载玩家战棋
            int posCount = m_BattleData.playerBrithPos.Count;
            int playerCount = m_BattleData.maxPlayerBattleUnit;
            for (int i = 0; i < Mathf.Min(posCount, playerCount); ++i)
            {
                int typeId = 10000 + Random.Range(1, 6);
                Vector2Int pos = m_BattleData.playerBrithPos[i];

                BattleUnitData battleUnitData = new BattleUnitData(typeId, mapEntityId, pos, CampType.Player);
                GameEntry.Entity.ShowBattleUnit(battleUnitData);
            }
        }

        private void SelectPlayerBattleUnit()
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

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;

            if (ne.EntityLogicType == typeof(GridMap))
            {
                gridMap = ne.Entity.Logic as GridMap;
                InitBattleUnit(gridMap.Id);
            }
        }

        private void OnGridUnitDead(object sender, GameEventArgs e)
        {
            GridUnitDeadEventArgs ne = (GridUnitDeadEventArgs)e;

            if (ne.gridUnit.Data.GridUnitType == GridUnitType.BattleUnit)
            {
                var battleUnitList = gridMap.GetBattleUnitList(ne.gridUnit.Data.CampType);
                if (battleUnitList.Count == 0)
                {
                    m_BattleEnd = true;
                    BattleResultInfo info = null;
                    if (ne.gridUnit.Data.CampType == CampType.Player)
                    {
                        info = new BattleResultInfo(CampType.Enemy);
                    }
                    else if(ne.gridUnit.Data.CampType == CampType.Enemy)
                    {
                        info = new BattleResultInfo(CampType.Player);
                    }
                    GameEntry.DataNode.SetData("BattleResultInfo", info);
                    GameEntry.Fsm.DestroyFsm(m_BattleFsm);
                }
            }
            GameEntry.Entity.HideEntity(ne.gridUnit);
        }

        public bool NeedRoundSwitch()
        {
            List<BattleUnit> battleUnits = gridMap.GetBattleUnitList(activeCamp);
            foreach (var battleUnit in battleUnits)
            {
                if (battleUnit.CanAction)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
