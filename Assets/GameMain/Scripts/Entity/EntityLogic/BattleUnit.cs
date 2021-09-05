using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;

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

            switch (m_Data.CampType)
            {
                case CampType.Player: spriteRenderer.color = playerColor; break;
                case CampType.Enemy: spriteRenderer.color = enemyColor; break;
            }
        }
    }
}
