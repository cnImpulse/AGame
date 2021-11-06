using System;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using Bright.Serialization;

namespace SSRPG
{
    public class CfgComponent : GameFrameworkComponent
    {
        [NonSerialized]
        public bool EndLoad = false;

        private Dictionary<string, ByteBuf> ByteBufList = new Dictionary<string, ByteBuf>();

        public Cfg.Tables Tables
        {
            get;
            private set;
        }

        public void LoadTables()
        {
            for (int i = 0; i < Cfg.Tables.Assets.Length; ++i)
            {
                GameEntry.Resource.LoadAsset($"Assets/GameMain/GameData/CfgData/{Cfg.Tables.Assets[i]}.bytes",
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

            if (!EndLoad && ByteBufList.Count == Cfg.Tables.Assets.Length)
            {
                Tables = new Cfg.Tables(LoadByteBuf);
                EndLoad = true;
            }
        }
    }
}
