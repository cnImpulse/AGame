using System.Collections.Generic;
using UnityEngine;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using Bright.Serialization;

namespace SSRPG
{
    public class CfgComponent : GameFrameworkComponent
    {
        private Dictionary<string, ByteBuf> ByteBufList = new Dictionary<string, ByteBuf>();

        public bool EndLoad = false;

        public cfg.Tables Tables
        {
            get;
            private set;
        }

        public void LoadTables()
        {
            for (int i = 0; i < cfg.Tables.Assets.Length; ++i)
            {
                GameEntry.Resource.LoadAsset($"Assets/GameMain/GameData/CfgData/{cfg.Tables.Assets[i]}.bytes",
                    new LoadAssetCallbacks(OnAssetLoadScuess));
            }
        }

        private ByteBuf LoadByteBuf(string file)
        {
            return ByteBufList[file];
        }

        public void OnAssetLoadScuess(string assetName, object asset, float duration, object userData)
        {
            TextAsset textAsset = asset as TextAsset;
            ByteBufList.Add(textAsset.name, new ByteBuf(textAsset.bytes));

            if (!EndLoad && ByteBufList.Count == cfg.Tables.Assets.Length)
            {
                Tables = new cfg.Tables(LoadByteBuf);
                EndLoad = true;
            }
        }
    }
}
