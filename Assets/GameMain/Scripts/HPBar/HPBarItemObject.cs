using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace SSRPG
{
    public class HPBarItemObject : ObjectBase
    {
        public static HPBarItemObject Create(object target)
        {
            HPBarItemObject hpBarItemObject = ReferencePool.Acquire<HPBarItemObject>();
            hpBarItemObject.Initialize(target);
            return hpBarItemObject;
        }

        protected override void Release(bool isShutdown)
        {
            HPBarItem hpBarItem = (HPBarItem)Target;
            if (hpBarItem == null)
            {
                return;
            }

            Object.Destroy(hpBarItem.gameObject);
        }
    }
}
