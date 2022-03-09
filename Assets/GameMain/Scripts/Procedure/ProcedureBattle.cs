using UnityEngine;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace SSRPG
{
    public class ProcedureBattle : ProcedureBase
    {
        private bool m_BattleEnd = false;

        private LevelData m_LevelData = null;
        private BattleForm m_BattleForm = null;
        private IFsm<ProcedureBattle> m_Fsm = null;

        public GridMap GridMap { get; private set; }

        public void StartBattle()
        {
            GameEntry.Effect.HideGridMapEffect();
            InitBattleFsm();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowGirdMapSuccess);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(EventName.GridUnitDead, OnGridUnitDead);

            InitBattle(GameEntry.Battle.BattleId);
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
            GridMap = null;
            m_BattleEnd = false;
            m_LevelData = null;

            GameEntry.UI.CloseUIForm(m_BattleForm, isShutdown);
            m_BattleForm = null;

            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowGirdMapSuccess);
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Unsubscribe(EventName.GridUnitDead, OnGridUnitDead);
        }

        private void InitBattle(int levelId)
        {
            string path = AssetUtl.GetLevelData(levelId);
            GameEntry.Resource.LoadAsset(path, (assetName, asset, duration, userData) =>
            {
                TextAsset textAsset = asset as TextAsset;
                m_LevelData = Utility.Json.ToObject<LevelData>(textAsset.text);
                GameEntry.Entity.ShowGridMap(m_LevelData.MapId);
            });
        }

        private void InitBattleUnit()
        {
            foreach (var enemy in m_LevelData.EnemyList)
            {
                var typeId = enemy.Value;
                var gridData = GridMap.Data.GetGridData(enemy.Key);
                var roleData = new RoleData(typeId);
                roleData.UpLevel(m_LevelData.MapLevel);
                GridMap.RegisterBattleUnit(roleData, gridData.GridPos, CampType.Enemy);
            }

            int posCount = m_LevelData.PlayerBrithList.Count;
            foreach(var position in m_LevelData.PlayerBrithList)
            {
                var typeId = 1000 + Random.Range(1, 6);
                var roleData = new RoleData(typeId);
                roleData.UpLevel(m_LevelData.MapLevel);
                GridMap.RegisterBattleUnit(roleData, position, CampType.Player);
            }
        }

        private void InitBattleForm()
        {
            GameEntry.UI.OpenUIForm(Cfg.UI.FormType.BattleForm, this);
        }

        private void InitBattleFsm()
        {
            m_Fsm = GameEntry.Fsm.CreateFsm(this, new BattleStartState(), new RoundStartState(),
                new BattleUnitSelectState(), new BattleState(), new RoundEndState(), new BattleEndState());
            m_Fsm.Start<BattleStartState>();
        }

        private void OnShowGirdMapSuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.Entity.Logic is GridMap)
            {
                GridMap = ne.Entity.Logic as GridMap;
                GameEntry.Effect.ShowGridEffect(m_LevelData.PlayerBrithList, Cfg.Effect.GridEffectType.Brith);
                InitBattleUnit();
                InitBattleForm();
            }
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UIForm.Logic is BattleForm)
            {
                m_BattleForm = ne.UIForm.Logic as BattleForm;
            }
        }

        private void OnGridUnitDead(object sender, GameEventArgs e)
        {
            var data = sender as GridUnitData;

            GridMap.UnRegisterGridUnit(GameEntry.Entity.GetEntityLogic<GridUnit>(data.Id));
            if (data.GridUnitType == GridUnitType.BattleUnit)
            {
                var battleUnitList = GridMap.GetGridUnitList<BattleUnit>(data.CampType);
                if (battleUnitList.Count == 0)
                {
                    m_BattleEnd = true;
                    BattleResultInfo info = new BattleResultInfo(BattleUtl.GetHostileCamp(data.CampType));
                    GameEntry.DataNode.SetData("BattleResultInfo", info);
                    GameEntry.Fsm.DestroyFsm(m_Fsm);
                }
            }
        }
    }
}
