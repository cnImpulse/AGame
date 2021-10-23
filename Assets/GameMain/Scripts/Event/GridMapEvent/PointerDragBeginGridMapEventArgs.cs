using UnityEngine;
using GameFramework;
using GameFramework.Event;

namespace SSRPG
{
    /// <summary>
    /// 网格地图拖动开始事件
    /// </summary>
    public class PointerDragBeginGridMapEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PointerDragBeginGridMapEventArgs).GetHashCode();

        public PointerDragBeginGridMapEventArgs()
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

        public static PointerDragBeginGridMapEventArgs Create(GridData gridData)
        {
            PointerDragBeginGridMapEventArgs e = ReferencePool.Acquire<PointerDragBeginGridMapEventArgs>();
            e.gridData = gridData;
            return e;
        }

        public override void Clear()
        {
            gridData = null;
        }
    }
}