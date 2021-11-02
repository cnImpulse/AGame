using UnityEngine;

namespace SSRPG
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        /// <summary>
        /// 配置表组件
        /// </summary>
        public static CfgComponent Cfg
        {
            get;
            private set;
        }

        /// <summary>
        /// 特效组件
        /// </summary>
        public static EffectComponent Effect
        {
            get;
            private set;
        }

        /// <summary>
        /// 导航器组件
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
        /// 技能组件
        /// </summary>
        public static SkillComponent Skill
        {
            get;
            private set;
        }

        /// <summary>
        /// 战斗组件
        /// </summary>
        public static BattleComponent Battle
        {
            get;
            private set;
        }

        /// <summary>
        /// 游戏提示组件
        /// </summary>
        public static GameTipsComponent GameTips
        {
            get;
            private set;
        }

        /// <summary>
        /// 存档组件
        /// </summary>
        public static SaveComponent Save
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            Cfg             = UnityGameFramework.Runtime.GameEntry.GetComponent<CfgComponent>();
            Skill           = UnityGameFramework.Runtime.GameEntry.GetComponent<SkillComponent>();
            Battle          = UnityGameFramework.Runtime.GameEntry.GetComponent<BattleComponent>();
            Effect          = UnityGameFramework.Runtime.GameEntry.GetComponent<EffectComponent>();
            Navigator       = UnityGameFramework.Runtime.GameEntry.GetComponent<NavigatorComponent>();
            GridUnitInfo    = UnityGameFramework.Runtime.GameEntry.GetComponent<GridUnitInfoComponent>();
            GameTips        = UnityGameFramework.Runtime.GameEntry.GetComponent<GameTipsComponent>();
            Save            = UnityGameFramework.Runtime.GameEntry.GetComponent<SaveComponent>();
        }
    }
}
