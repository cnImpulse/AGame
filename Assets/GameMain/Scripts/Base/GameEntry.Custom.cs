﻿using UnityEngine;

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

        public static HPBarComponent HPBar
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            Effect      = UnityGameFramework.Runtime.GameEntry.GetComponent<EffectComponent>();
            Navigator   = UnityGameFramework.Runtime.GameEntry.GetComponent<NavigatorComponent>();
            HPBar       = UnityGameFramework.Runtime.GameEntry.GetComponent<HPBarComponent>();
        }
    }
}
