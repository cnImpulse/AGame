using System;
using UnityEngine;

namespace SSRPG
{
    [Serializable]
    public abstract class ChirldEntityData : EntityData
    {
        [SerializeField]
        private int m_ParentId = 0;

        public ChirldEntityData(int typeId, int parentId)
            : base(typeId)
        {
            m_ParentId = parentId;
        }

        /// <summary>
        /// 父实体编号。
        /// </summary>
        public int ParentId => m_ParentId;
    }
}
