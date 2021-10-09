using UnityEngine;

namespace SSRPG
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        /// <summary>
        /// 获取特效组件
        /// </summary>
        public static EffectComponent Effect
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取导航器组件
        /// </summary>
        public static NavigatorComponent Navigator
        {
            get;
            private set;
        }

        public static GridUnitInfoComponent GridUnitInfo
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取技能组件
        /// </summary>
        public static SkillComponent Skill
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取战斗组件
        /// </summary>
        public static BattleComponent Battle
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            Skill           = UnityGameFramework.Runtime.GameEntry.GetComponent<SkillComponent>();
            Battle          = UnityGameFramework.Runtime.GameEntry.GetComponent<BattleComponent>();
            Effect          = UnityGameFramework.Runtime.GameEntry.GetComponent<EffectComponent>();
            Navigator       = UnityGameFramework.Runtime.GameEntry.GetComponent<NavigatorComponent>();
            GridUnitInfo    = UnityGameFramework.Runtime.GameEntry.GetComponent<GridUnitInfoComponent>();
        }
    }
}
