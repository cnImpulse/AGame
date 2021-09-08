using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class EffectComponent : GameFrameworkComponent
    {
        [SerializeField]
        private Transform m_EffectInstanceRoot = null;

        [SerializeField]
        private Dictionary<int, EffectBase> m_EffectList = null;

        private void Start()
        {
            if (m_EffectInstanceRoot == null)
            {
                Log.Error("You must set effect instance root first.");
                return;
            }

            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnCreatEffect);

            m_EffectList = new Dictionary<int, EffectBase>();
        }

        private void Update()
        {
            
        }

        public int CreatEffect(EffectType type, Vector3 position)
        {
            int entityId = GameEntry.Entity.GenerateSerialId();
            EffectDataBase effectData = new EffectDataBase(entityId, (int)type, position);

            GameEntry.Entity.ShowEffect(effectData);
            return entityId;
        }

        private void OnCreatEffect(object sender, GameFrameworkEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType != typeof(EffectBase))
            {
                return;
            }

            m_EffectList.Add(ne.Entity.Id, ne.Entity.Logic as EffectBase);
            ne.Entity.transform.SetParent(m_EffectInstanceRoot);
        }

        private void OnDestroy()
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnCreatEffect);
        }
    }
}