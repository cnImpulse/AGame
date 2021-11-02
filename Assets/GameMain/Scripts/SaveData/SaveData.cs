using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSRPG
{
    public class SaveData
    {
        public int Id = 0;
        public Time CreatTime = default;
        public bool EndFirstGuide = false;

        public Dictionary<int, int> DiscipleList = new Dictionary<int, int>();
    }
}
