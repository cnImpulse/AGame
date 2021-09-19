using UnityEngine;
using GameFramework;
using GameFramework.Event;

namespace SSRPG
{
    /// <summary>
    /// 战斗回合切换事件
    /// </summary>
    public class GridUnitDeadEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(GridUnitDeadEventArgs).GetHashCode();

        public GridUnitDeadEventArgs()
        {
            gridUnit = null;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public GridUnit gridUnit
        {
            get;
            private set;
        }

        public static GridUnitDeadEventArgs Create(GridUnit gridUnit)
        {
            GridUnitDeadEventArgs e = ReferencePool.Acquire<GridUnitDeadEventArgs>();
            e.gridUnit = gridUnit;

            return e;
        }

        public override void Clear()
        {
            gridUnit = null;
        }
    }
}