using System.IO;
using UnityEngine;
using UnityGameFramework.Runtime;
using Bright.Serialization;

namespace SSRPG
{
    public class CfgComponent : GameFrameworkComponent
    {
        public cfg.Tables Tables
        {
            get;
            private set;
        }

        public void LoadTables()
        {
            Tables = new cfg.Tables(LoadByteBuf);
        }

        private static ByteBuf LoadByteBuf(string file)
        {
            return new ByteBuf(File.ReadAllBytes($"{Application.dataPath}/GameMain/GameData/CfgData/{file}.bin"));
        }
    }
}
