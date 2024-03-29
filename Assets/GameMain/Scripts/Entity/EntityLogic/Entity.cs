﻿using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using System;
using System.Collections.Generic;

namespace SSRPG
{
    public abstract class Entity : EntityLogic
    {
        [SerializeField]
        private EntityData m_Data = null;

        public int Id
        {
            get
            {
                return Entity.Id;
            }
        }

        protected void InitLayer(string layerName)
        {
            gameObject.SetLayerRecursively(LayerMask.NameToLayer(layerName));

            var renderers = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; ++i)
            {
                renderers[i].sortingLayerID = SortingLayer.NameToID(layerName);
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_Data = userData as EntityData;
            if (m_Data == null)
            {
                Log.Error("Entity data is invalid!");
                return;
            }
            
            Name = Utility.Text.Format("[{0}-{1}]", m_Data.Name, Id);
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
        }

        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }
}
