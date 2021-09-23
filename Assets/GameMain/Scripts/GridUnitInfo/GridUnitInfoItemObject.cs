using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace SSRPG
{
    public class GridUnitInfoItemObject : ObjectBase
    {
        public static GridUnitInfoItemObject Create(object target)
        {
            GridUnitInfoItemObject infoItemObject = ReferencePool.Acquire<GridUnitInfoItemObject>();
            infoItemObject.Initialize(target);
            return infoItemObject;
        }

        protected override void Release(bool isShutdown)
        {
            GridUnitInfoItem gridUnitInfoItem = (GridUnitInfoItem)Target;
            if (gridUnitInfoItem == null)
            {
                return;
            }

            Object.Destroy(gridUnitInfoItem.gameObject);
        }
    }
}
