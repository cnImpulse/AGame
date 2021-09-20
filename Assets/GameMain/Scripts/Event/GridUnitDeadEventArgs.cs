using UnityEngine;
using GameFramework;
using GameFramework.Event;

namespace SSRPG
{
    /// <summary>
    /// 网格单位死亡事件
    /// </summary>
    public class GridUnitDeadEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(GridUnitDeadEventArgs).GetHashCode();

        public GridUnitDeadEventArgs()
        {
            gridUnitData = null;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public GridUnitData gridUnitData
        {
            get;
            private set;
        }

        public static GridUnitDeadEventArgs Create(GridUnitData gridUnitData)
        {
            GridUnitDeadEventArgs e = ReferencePool.Acquire<GridUnitDeadEventArgs>();
            e.gridUnitData = gridUnitData;

            return e;
        }

        public override void Clear()
        {
            gridUnitData = null;
        }
    }
}