//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

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
        /// 拥有者编号。
        /// </summary>
        public int ParentId
        {
            get
            {
                return m_ParentId;
            }
        }
    }
}
