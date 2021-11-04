using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using UnityEngine.Tilemaps;

namespace SSRPG
{
    public class EffectComponent : GameFrameworkComponent
    {
        [SerializeField]
        private Transform m_EffectInstanceRoot = null;

        [SerializeField]
        private Dictionary<int, EffectBase> m_EffectList = null;

        public Tilemap GridMapEffect = null;

        private void Start()
        {
            if (m_EffectInstanceRoot == null)
            {
                Log.Error("You must set effect instance root first.");
                return;
            }

            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnCreatEffect);

            GridMapEffect = GetComponentInChildren<Tilemap>();
            m_EffectList = new Dictionary<int, EffectBase>();
        }

        private void Update()
        {
            
        }

        /// <summary>
        /// 创建特效
        /// </summary>
        /// <param name="type">特效类型</param>
        /// <param name="position">特效位置</param>
        /// <returns>特效实体Id</returns>
        public int CreatEffect(EffectType type, Vector3 position)
        {
            int entityId = GameEntry.Entity.GenerateSerialId();
            EffectDataBase effectData = new EffectDataBase(entityId, (int)type, position);

            GameEntry.Entity.ShowEffect(effectData);
            return entityId;
        }

        public int CreatUIEffect(UIEffectType type, Vector3 position)
        {
            return 0;
        }

        /// <summary>
        /// 销毁特效
        /// </summary>
        /// <param name="entityId">特效实体Id</param>
        public void DestoryEffect(int entityId)
        {
            if (entityId == 0)
            {
                return;
            }
            GameEntry.Entity.HideEntity(entityId);
        }

        /// <summary>
        /// 改变特效位置
        /// </summary>
        /// <param name="entityId">特效实体Id</param>
        /// <param name="position">特效位置</param>
        public bool ChangeEffectPos(int entityId, Vector3 position)
        {
            if (m_EffectList.TryGetValue(entityId, out var effect))
            {
                effect.transform.position = position;
                return true;
            }
            return false;
        }

        private void OnCreatEffect(object sender, GameFrameworkEventArgs e)
        {
            var ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType != typeof(EffectBase))
            {
                return;
            }

            m_EffectList.Add(ne.Entity.Id, ne.Entity.Logic as EffectBase);
            ne.Entity.transform.SetParent(m_EffectInstanceRoot);
        }
    }
}