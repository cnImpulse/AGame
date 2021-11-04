using GameFramework;
using UnityEngine;
using System.Collections.Generic;

namespace SSRPG
{
    public static class Utl
    {
        public static string GetText(CampType type, string text)
        {
            if (type == CampType.Player)
            {
                return string.Format("<color={0}>{1}</color>", "#70FFF0", text);
            }
            else if (type == CampType.Enemy)
            {
                return string.Format("<color={0}>{1}</color>", "#FF7070", text);
            }
            else
            {
                return text;
            }
        }
    }
}