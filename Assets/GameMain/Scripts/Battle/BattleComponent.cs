using System;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class BattleComponent : GameFrameworkComponent
    {
        public int BattleId = 2;
        public bool AutoBattle = true;

        private int m_SelectEffectId = 0;
        private GridMap m_GridMap = null;
        private List<Vector2Int> m_SelectEffectArea = null;

        private void Start()
        {
            GameEntry.Event.Subscribe(EventName.GridUnitDead, OnGridUnitDead);
        }

        private void Update()
        {
            if (m_SelectEffectArea == null || m_GridMap == null)
            {
                return;
            }

            var gridPos = m_GridMap.WorldPosToGridPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (m_SelectEffectArea.Contains(gridPos))
            {
                GameEntry.Battle.ShowSelectEffect(m_GridMap.GridPosToWorldPos(gridPos));
            }
            else
            {
                GameEntry.Battle.HideSelectEffect();
            }
        }

        public void SetAreaSelectEffect(List<Vector2Int> area, GridMap gridMap)
        {
            m_SelectEffectArea = area;
            m_GridMap = gridMap;
        }

        public void HideAreaSelectEffect()
        {
            m_SelectEffectArea = null;
            m_GridMap = null;
            HideSelectEffect();
        }

        public void ShowSelectEffect(Vector3 position)
        {
            var entity = GameEntry.Effect.GetEffect(m_SelectEffectId);
            if (entity == null)
            {
                m_SelectEffectId = GameEntry.Effect.CreatEffect(Cfg.Effect.EffectType.Select, position);
            }
            else
            {
                entity.Position = position;
            }
        }

        public void HideSelectEffect()
        {
            GameEntry.Effect.HideEffect(m_SelectEffectId);
            m_SelectEffectId = 0;
        }

        private void OnGridUnitDead(object sender, GameEventArgs e)
        {
            var ne = e as GameEventBase;

        }
    }
}
