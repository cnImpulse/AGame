using UnityEngine;
using GameFramework;
using GameFramework.Event;

namespace SSRPG
{
    /// <summary>
    /// 网格地图点击事件
    /// </summary>
    public class PointerDownGridMapEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PointerDownGridMapEventArgs).GetHashCode();

        public PointerDownGridMapEventArgs()
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
        /// 网格单元数据
        /// </summary>
        public GridData gridData
        {
            get;
            private set;
        }

        public static PointerDownGridMapEventArgs Create(GridData gridData)
        {
            PointerDownGridMapEventArgs e = ReferencePool.Acquire<PointerDownGridMapEventArgs>();
            e.gridData = gridData;
            return e;
        }

        public override void Clear()
        {
            gridData = null;
        }
    }
}