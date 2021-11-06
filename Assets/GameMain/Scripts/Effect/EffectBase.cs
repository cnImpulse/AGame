using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class Effect : Entity
    {
        private EffectData m_Data;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_Data = userData as EffectData;
            if (m_Data == null)
            {
                Log.Error("EffectDataBase object data is invalid.");
                return;
            }

            transform.position = m_Data.Position;
        }
    }
}