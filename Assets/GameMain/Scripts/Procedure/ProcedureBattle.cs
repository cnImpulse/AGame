using UnityEngine;
using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace SSRPG
{
    public class ProcedureBattle : ProcedureBase
    {
        private SelectBattleUnitForm m_Form = null;

        public BattleData m_BattleData = null;

        public void StartBattle()
        {
            Log.Info("战斗开始。");

            m_Form.Close();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            Log.Info("进入战斗准备阶段。");

            InitBattle();
            SelectPlayerBattleUnit();
        }

        private void InitBattle()
        {
            int battleId = 1;
            string path = AssetUtl.GetBattleDataPath(battleId);
            m_BattleData = AssetUtl.LoadJsonData<BattleData>(path);

            GridMapData gridMapData = new GridMapData(m_BattleData.mapId);
            GameEntry.Entity.ShowGridMap(gridMapData);

            for (int i = 0; i < m_BattleData.enemyIds.Count; ++i)
            {
                int typeId = m_BattleData.enemyIds[i];
                Vector2Int pos = m_BattleData.enemyPos[i];

                BattleUnitData battleUnitData = new BattleUnitData(typeId, gridMapData.Id, pos, CampType.Enemy);
                GameEntry.Entity.ShowBattleUnit(battleUnitData);
            }
        }

        private void SelectPlayerBattleUnit()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SelectBattleUnitForm, this);
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_Form = (SelectBattleUnitForm)ne.UIForm.Logic;
        }
    }
}
