using System.Collections.Generic;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class SkillComponent : GameFrameworkComponent
    {
        public bool RequestReleaseSkill(int skillId, int casterId, int targetId)
        {
            BattleUnitSkill skill = new BattleUnitSkill(skillId, casterId, targetId);
            return skill.Release();
        }
    }
}
