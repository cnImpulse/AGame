using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class Effect : Entity
    {
        private EffectData m_Data = null;

        private float m_RealLifeTime = 0;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            InitLayer("Effect");
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_RealLifeTime = 0;
            m_Data = userData as EffectData;
            transform.position = m_Data.Position;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (m_Data.LifeTime == -1)
            {
                return;
            }

            m_RealLifeTime += elapseSeconds;
            if (m_RealLifeTime >= m_Data.LifeTime)
            {
                GameEntry.Effect.HideEffect(Id);
            }
        }
    }
}