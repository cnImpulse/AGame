using GameFramework;
using UnityEngine;
using System.Collections.Generic;

namespace SSRPG
{
    public static class BattleUtl
    {
        public static CampType GetHostileCamp(CampType campType)
        {
            if (campType == CampType.Enemy)
            {
                return CampType.Player;
            }
            else if (campType == CampType.Player)
            {
                return CampType.Enemy;
            }

            return CampType.None;
        }
    }
}