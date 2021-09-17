using System.Collections.Generic;
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
        private BattleData m_BattleData = null;

        private BattleForm m_Form = null;

        private IFsm<ProcedureBattle> m_BattleFsm = null;

        public GridMap gridMap;
        public List<BattleUnit> playerBattleUnits = null, enemyBattleUnits = null;
        public CampType activeCamp = CampType.None;

        public void StartBattle()
        {
            Log.Info("战斗开始。");

            m_Form.Close();
            m_Form = null;
            m_BattleFsm.Start<RoundSwitchState>();
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            playerBattleUnits = new List<BattleUnit>();
            enemyBattleUnits = new List<BattleUnit>();
            m_BattleFsm = GameEntry.Fsm.CreateFsm(this, new RoundSwitchState(), new SelectBattleUnitState(),
                new BattleUnitMoveState(), new BattleUnitActionState(), new BattleUnitAttackState(), new BattleUnitEndActionState());
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            Log.Info("进入战斗准备阶段。");

            InitBattle();
            SelectPlayerBattleUnit();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
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

            GridMapData gridMapData = new GridMapData(m_BattleData.mapId);
            GameEntry.Entity.ShowGridMap(gridMapData);

            // 加载敌人
            for (int i = 0; i < m_BattleData.enemyIds.Count; ++i)
            {
                int typeId = m_BattleData.enemyIds[i];
                Vector2Int pos = m_BattleData.enemyPos[i];

                BattleUnitData battleUnitData = new BattleUnitData(typeId, gridMapData.Id, pos, CampType.Enemy);
                GameEntry.Entity.ShowBattleUnit(battleUnitData);
            }

            // 加载玩家战棋
            int posCount = m_BattleData.playerBrithPos.Count;
            int playerCount = m_BattleData.maxPlayerBattleUnit;
            for (int i = 0; i < Mathf.Min(posCount, playerCount) ; ++i)
            {
                int typeId = 20000;
                Vector2Int pos = m_BattleData.playerBrithPos[i];

                BattleUnitData battleUnitData = new BattleUnitData(typeId, gridMapData.Id, pos, CampType.Player);
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

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;

            if (ne.EntityLogicType == typeof(GridMap))
            {
                gridMap = ne.Entity.Logic as GridMap;
            }
            else if (ne.EntityLogicType == typeof(BattleUnit))
            {
                BattleUnit battleUnit = ne.Entity.Logic as BattleUnit;
                if (battleUnit.BattleUnitData.CampType == CampType.Player)
                {
                    playerBattleUnits.Add(battleUnit);
                }
                else if (battleUnit.BattleUnitData.CampType == CampType.Enemy)
                {
                    enemyBattleUnits.Add(battleUnit);
                }
            }
        }

        public bool NeedRoundSwitch()
        {
            List<BattleUnit> battleUnits = GetActiveBattleUnitList();
            foreach (var battleUnit in battleUnits)
            {
                if (battleUnit.CanAction)
                {
                    return false;
                }
            }
            return true;
        }

        public List<BattleUnit> GetActiveBattleUnitList()
        {
            if (activeCamp == CampType.Player)
            {
                return playerBattleUnits;
            }
            else if (activeCamp == CampType.Enemy)
            {
                return enemyBattleUnits;
            }
            return null;
        }
    }
}
