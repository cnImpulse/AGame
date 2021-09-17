using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using GameFramework.Event;

namespace SSRPG
{
    /// <summary>
    /// 战斗单位。
    /// </summary>
    public class BattleUnit : GridUnit
    {
        private static Color enemyColor, playerColor;

        [SerializeField]
        private BattleUnitData m_Data;

        private SpriteRenderer spriteRenderer = null;

        public BattleUnitData BattleUnitData => m_Data;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            spriteRenderer = GetComponent<SpriteRenderer>();
            ColorUtility.TryParseHtmlString("#70FFF0", out playerColor);
            ColorUtility.TryParseHtmlString("#FF7070", out enemyColor);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_Data = userData as BattleUnitData;
            if (m_Data == null)
            {
                Log.Error("BattleUnit object data is invalid.");
                return;
            }

            CanAction = false;
            switch (m_Data.CampType)
            {
                case CampType.Player: spriteRenderer.color = playerColor; break;
                case CampType.Enemy: spriteRenderer.color = enemyColor; break;
            }

            GameEntry.Event.Subscribe(RoundSwitchEventArgs.EventId, OnRoundSwitch);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            GameEntry.Event.Unsubscribe(RoundSwitchEventArgs.EventId, OnRoundSwitch);
        }

        public void Move(Vector2Int destination)
        {
            m_GridMap.MoveTo(this, destination);
            transform.position = m_GridMap.GridPosToWorldPos(destination);
        }

        public void Attack(GridData gridData)
        {
            if (gridData == null || gridData.GridUnit == null)
            {
                return;
            }

            GridUnit gridUnit = gridData.GridUnit;
            gridUnit.BeAttack(m_Data.ATK);

            OnEndAction();
        }

        public bool CanAction
        {
            get;
            private set;
        }

        public void OnRoundSwitch(object sender, GameEventArgs e)
        {
            RoundSwitchEventArgs ne = (RoundSwitchEventArgs)e;
            if (ne.ActionCamp == m_Data.CampType)
            {
                OnRoundBegin();
            }
            else if (ne.EndActionCamp == m_Data.CampType)
            {
                OnEndAction();
            }
        }

        private void OnRoundBegin()
        {
            CanAction = true;
        }

        public void OnEndAction()
        {
            CanAction = false;
        }
    }
}
