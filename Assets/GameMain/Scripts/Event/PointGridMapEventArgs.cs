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
            position = default;
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
        public Vector3 position
        {
            get;
            private set;
        }

        public static PointGridMapEventArgs Create(Vector3 position)
        {
            PointGridMapEventArgs e = ReferencePool.Acquire<PointGridMapEventArgs>();
            e.position = position;
            return e;
        }

        public override void Clear()
        {
            position = default;
        }
    }
}