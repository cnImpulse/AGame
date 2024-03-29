﻿using System;
using UnityEngine;
using Newtonsoft.Json;

namespace SSRPG
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class EntityData
    {
        [SerializeField]
        private int m_Id = 0;

        [JsonProperty]
        [SerializeField]
        private int m_TypeId = 0;

        [SerializeField]
        private string m_Name = "";

        [JsonConstructor]
        public EntityData(int typeId)
        {
            m_Id = GameEntry.Entity.GenerateSerialId();
            m_TypeId = typeId;
            m_Name = "Entity";
        }

        public EntityData(int id, int typeId)
        {
            m_Id = id;
            m_TypeId = typeId;
            m_Name = "Entity";
        }

        /// <summary>
        /// 实体编号。
        /// </summary>
        public int Id => m_Id;

        /// <summary>
        /// 实体类型编号。
        /// </summary>
        public int TypeId => m_TypeId;

        /// <summary>
        /// 单位名称。
        /// </summary>
        public string Name
        {
            get => m_Name;
            set => m_Name = value;
        }
    }
}


