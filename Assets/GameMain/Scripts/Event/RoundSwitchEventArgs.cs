using UnityEngine;
using GameFramework;
using GameFramework.Event;

namespace SSRPG
{
    /// <summary>
    /// 战斗回合切换事件
    /// </summary>
    public class RoundSwitchEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(RoundSwitchEventArgs).GetHashCode();

        public RoundSwitchEventArgs()
        {
            EndActionCamp = CampType.None;
            ActionCamp = CampType.None;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public CampType EndActionCamp
        {
            get;
            private set;
        }

        public CampType ActionCamp
        {
            get;
            private set;
        }

        public static RoundSwitchEventArgs Create(CampType endActionCamp, CampType actionCamp)
        {
            RoundSwitchEventArgs e = ReferencePool.Acquire<RoundSwitchEventArgs>();
            e.EndActionCamp = endActionCamp;
            e.ActionCamp = actionCamp;
            
            return e;
        }

        public override void Clear()
        {
            EndActionCamp = CampType.None;
            ActionCamp = CampType.None;
        }
    }
}