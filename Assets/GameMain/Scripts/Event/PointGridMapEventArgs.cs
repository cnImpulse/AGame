using UnityEngine;
using GameFramework;
using GameFramework.Event;

namespace SSRPG
{
    /// <summary>
    /// 网格地图点击事件
    /// </summary>
    public class PointGridMapEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PointGridMapEventArgs).GetHashCode();

        public PointGridMapEventArgs()
        {
            gridData = null;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 点击的网格数据
        /// </summary>
        public GridData gridData
        {
            get;
            private set;
        }

        public static PointGridMapEventArgs Create(GridData gridData)
        {
            PointGridMapEventArgs e = ReferencePool.Acquire<PointGridMapEventArgs>();
            e.gridData = gridData;
            return e;
        }

        public override void Clear()
        {
            gridData = null;
        }
    }
}