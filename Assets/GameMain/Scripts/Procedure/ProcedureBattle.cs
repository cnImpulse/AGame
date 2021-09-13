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
        private IFsm<ProcedureBattle> m_BattleFsm = null;
        private BattleForm m_Form = null;

        private BattleData m_BattleData = null;

        public GridMapData GridMapData = null;
        public CampType ActiveCamp = CampType.Player;

        public void StartBattle()
        {
            Log.Info("战斗开始。");

            m_Form.Close();
            m_Form = null;
            m_BattleFsm.Start<SelectBattleUnitState>();
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            m_BattleFsm = GameEntry.Fsm.CreateFsm(this, new SelectBattleUnitState(), new BattleUnitMoveState(),
                new BattleUnitActionState(), new BattleUnitAttackState());
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            Log.Info("进入战斗准备阶段。");

            InitBattle();
            SelectPlayerBattleUnit();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            if (m_Form != null)
            {
                m_Form.Close(isShutdown);
                m_Form = null;
            }

            GameEntry.Fsm.DestroyFsm(m_BattleFsm);
        }

        private void InitBattle()
        {
            int battleId = 1;
            string path = AssetUtl.GetBattleDataPath(battleId);
            m_BattleData = AssetUtl.LoadJsonData<BattleData>(path);

            GridMapData = new GridMapData(m_BattleData.mapId);
            GameEntry.Entity.ShowGridMap(GridMapData);

            // 加载敌人
            for (int i = 0; i < m_BattleData.enemyIds.Count; ++i)
            {
                int typeId = m_BattleData.enemyIds[i];
                Vector2Int pos = m_BattleData.enemyPos[i];

                BattleUnitData battleUnitData = new BattleUnitData(typeId, GridMapData.Id, pos, CampType.Enemy);
                GameEntry.Entity.ShowBattleUnit(battleUnitData);
            }

            // 加载玩家战棋
            int posCount = m_BattleData.playerBrithPos.Count;
            int playerCount = m_BattleData.maxPlayerBattleUnit;
            for (int i = 0; i < Mathf.Min(posCount, playerCount) ; ++i)
            {
                int typeId = 20000;
                Vector2Int pos = m_BattleData.playerBrithPos[i];

                BattleUnitData battleUnitData = new BattleUnitData(typeId, GridMapData.Id, pos, CampType.Player);
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

            m_Form = (BattleForm)ne.UIForm.Logic;
        }
    }
}
