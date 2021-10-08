using UnityEngine;
using GameFramework;
using GameFramework.Event;

namespace SSRPG
{
    /// <summary>
    /// 网格单位受伤事件
    /// </summary>
    public class GridUnitDamageEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(GridUnitDamageEventArgs).GetHashCode();

        public GridUnitDamageEventArgs()
        {
            DamageInfo = null;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public DamageInfo DamageInfo
        {
            get;
            private set;
        }

        public static GridUnitDamageEventArgs Create(DamageInfo damageInfo)
        {
            GridUnitDamageEventArgs e = ReferencePool.Acquire<GridUnitDamageEventArgs>();
            e.DamageInfo = damageInfo;
            return e;
        }

        public override void Clear()
        {
            DamageInfo = null;
        }
    }
}