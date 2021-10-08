using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSRPG
{
    public class DamageInfo
    {
        public int DamageHP
        {
            get;
            private set;
        }

        public int CasterId
        {
            get;
            private set;
        }

        public int TargetId
        {
            get;
            private set;
        }

        public DamageInfo(int damageHP, int casterId, int targetId)
        {
            DamageHP = damageHP;
            CasterId = casterId;
            TargetId = targetId;
        }
    }
}
