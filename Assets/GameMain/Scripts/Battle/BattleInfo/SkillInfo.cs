using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSRPG
{
    public class SkillInfo
    {
        public int EffectValue { get; }
        public int CasterId { get; }
        public int TargetId { get; }

        public SkillInfo(int value, int casterId, int targetId)
        {
            EffectValue = value;
            CasterId = casterId;
            TargetId = targetId;
        }
    }
}
