using UnityEngine;
using GameFramework;
using GameFramework.Event;

namespace SSRPG
{
    /// <summary>
    /// 瓦片地图点击事件
    /// </summary>
    public class PointGridMapEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PointGridMapEventArgs).GetHashCode();

        public PointGridMapEventArgs()
        {
            PointPos = default;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 点击位置
        /// </summary>
        public Vector2Int PointPos
        {
            get;
            private set;
        }

        public static PointGridMapEventArgs Create(Vector2Int pointPos)
        {
            PointGridMapEventArgs e = ReferencePool.Acquire<PointGridMapEventArgs>();
            e.PointPos = pointPos;
            return e;
        }

        public override void Clear()
        {
            PointPos = default;
        }
    }
}