using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSRPG
{
    public class SaveData
    {
        public string SaveName = DateTime.Now.ToShortDateString();
        public bool EndFirstGuide = false;

        public Dictionary<int, int> DiscipleList = new Dictionary<int, int>();
    }
}
