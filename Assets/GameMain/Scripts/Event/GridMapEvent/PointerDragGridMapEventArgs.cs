using UnityEngine;
using GameFramework;
using GameFramework.Event;

namespace SSRPG
{
    /// <summary>
    /// 网格地图拖动事件
    /// </summary>
    public class PointerDragGridMapEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PointerDragGridMapEventArgs).GetHashCode();

        public PointerDragGridMapEventArgs()
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

        public static PointerDragGridMapEventArgs Create(GridData gridData)
        {
            PointerDragGridMapEventArgs e = ReferencePool.Acquire<PointerDragGridMapEventArgs>();
            e.gridData = gridData;
            return e;
        }

        public override void Clear()
        {
            gridData = null;
        }
    }
}