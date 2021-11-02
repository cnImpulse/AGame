using UnityEngine;
using GameFramework;
using GameFramework.Event;

namespace SSRPG
{
    /// <summary>
    /// 战斗回合切换事件
    /// </summary>
    public class EnsureRewardEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(EnsureRewardEventArgs).GetHashCode();

        public EnsureRewardEventArgs()
        {
            ItemId = 0;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int ItemId
        {
            get;
            private set;
        }

        public static EnsureRewardEventArgs Create(int itemId)
        {
            EnsureRewardEventArgs e = ReferencePool.Acquire<EnsureRewardEventArgs>();
            e.ItemId = itemId;

            return e;
        }

        public override void Clear()
        {
            ItemId = 0;
        }
    }
}