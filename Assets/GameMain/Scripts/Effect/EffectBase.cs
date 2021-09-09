using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class EffectBase : Entity
    {
        private EffectDataBase m_Data;

        public EffectBase()
        {

        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_Data = userData as EffectDataBase;
            if (m_Data == null)
            {
                Log.Error("EffectDataBase object data is invalid.");
                return;
            }

            transform.position = m_Data.Position;
        }
    }
}