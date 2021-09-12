using UnityEngine;
using GameFramework;
using GameFramework.Event;

namespace SSRPG
{
    /// <summary>
    /// 网格单位移动事件
    /// </summary>
    public class GridUnitMoveEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PointGridMapEventArgs).GetHashCode();

        public GridUnitMoveEventArgs()
        {
            destination = null;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 移动到终点
        /// </summary>
        public GridData destination
        {
            get;
            private set;
        }

        public static GridUnitMoveEventArgs Create(GridData destination)
        {
            GridUnitMoveEventArgs e = ReferencePool.Acquire<GridUnitMoveEventArgs>();
            e.destination = destination;
            return e;
        }

        public override void Clear()
        {
            destination = null;
        }
    }
}