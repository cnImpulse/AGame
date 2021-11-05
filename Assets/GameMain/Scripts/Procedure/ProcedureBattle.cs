using UnityEngine;
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

        private IFsm<ProcedureBattle> m_Fsm = null;

        public GridMap GridMap { get; private set; }

        public void StartBattle()
        {
            GameEntry.Effect.HideGridMapEffect();
            GameEntry.Battle.InitBattle(GridMap);
            InitBattleFsm();
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowGirdMapSuccess);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(GridUnitDeadEventArgs.EventId, OnGridUnitDead);

            
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

            GameEntry.Entity.HideEntity(GridMap);
            GameEntry.Fsm.DestroyFsm(m_Fsm);
            
            GridMap = null;
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
            m_Fsm = GameEntry.Fsm.CreateFsm(this, new BattleStartState(), new RoundStartState(),
                new BattleState(), new RoundEndState(), new BattleEndState());
            m_Fsm.Start<BattleStartState>();
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
                int typeId = enemy.Value;
                var gridData = GridMap.Data.GetGridData(enemy.Key);
                BattleUnitData battleUnitData = new BattleUnitData(typeId, gridData.GridPos, CampType.Enemy);
                GridMap.RegisterBattleUnit(battleUnitData);
            }

            // 加载玩家战棋
            int posCount = m_BattleData.playerBrithList.Count;
            foreach(var position in m_BattleData.playerBrithList)
            {
                int typeId = 10000 + Random.Range(1, 6);
                BattleUnitData battleUnitData = new BattleUnitData(typeId, position, CampType.Player);
                GridMap.RegisterBattleUnit(battleUnitData);
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
                GridMap = ne.Entity.Logic as GridMap;
                GameEntry.Effect.ShowGridMapEffect(m_BattleData.playerBrithList, GridMapEffectId.Brith);
                InitBattleUnit();
            }
        }

        private void OnGridUnitDead(object sender, GameEventArgs e)
        {
            GridUnitDeadEventArgs ne = (GridUnitDeadEventArgs)e;

            GridUnitData data = ne.gridUnit.Data;
            GridMap.UnRegisterGridUnit(ne.gridUnit);
            if (data.GridUnitType == GridUnitType.BattleUnit)
            {
                var battleUnitList = GridMap.GetGridUnitList<BattleUnit>(data.CampType);
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
                    GameEntry.Fsm.DestroyFsm(m_Fsm);
                }
            }
        }
    }
}
