using UnityEngine;
using GameFramework;
using GameFramework.Event;

namespace SSRPG
{
    /// <summary>
    /// 网格地图结束事件
    /// </summary>
    public class PointerDropGridMapEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PointerDropGridMapEventArgs).GetHashCode();

        public PointerDropGridMapEventArgs()
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

        public static PointerDropGridMapEventArgs Create(GridData gridData)
        {
            PointerDropGridMapEventArgs e = ReferencePool.Acquire<PointerDropGridMapEventArgs>();
            e.gridData = gridData;
            return e;
        }

        public override void Clear()
        {
            gridData = null;
        }
    }
}