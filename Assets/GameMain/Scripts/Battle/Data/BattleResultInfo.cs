using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class BattleResultInfo : VarObject
    {
        public CampType WinCampType
        {
            get;
            private set;
        }

        public BattleResultInfo(CampType winCampType)
        {
            WinCampType = winCampType;
        }
    }
}
