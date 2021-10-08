using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSRPG
{
    public abstract class SkillBase
    {
        protected int m_Id = 0;
        protected int m_CasterId = 0;
        protected int m_TargetId = 0;

        public SkillBase(int id, int casterId, int targetId)
        {
            m_Id = id;
            m_CasterId = casterId;
            m_TargetId = targetId;
        }

        protected abstract void OnRelease();
    }
}
