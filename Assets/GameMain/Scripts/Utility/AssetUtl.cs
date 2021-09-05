using System.IO;
using GameFramework;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace SSRPG
{
    public static class AssetUtl
    {
        public static string GetConfigAsset(string assetName, bool fromBytes)
        {
            return Utility.Text.Format("Assets/GameMain/Configs/{0}.{1}", assetName, fromBytes ? "bytes" : "txt");
        }

        public static string GetDataTableAsset(string assetName, bool fromBytes)
        {
            return Utility.Text.Format("Assets/GameMain/DataTables/{0}.{1}", assetName, fromBytes ? "bytes" : "txt");
        }

        public static string GetEntityAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Entities/{0}.prefab", assetName);
        }

        public static string GetTileAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Tilemap/{0}.asset", assetName);
        }

        public static string GetMapDataPath(int mapId)
        {
            return Utility.Text.Format("Assets/GameMain/GameData/MapData/MapData_{0}.json", mapId);
        }

        public static string GetBattleDataPath(int battleId)
        {
            return Utility.Text.Format("Assets/GameMain/GameData/BattleData/BattleData_{0}.json", battleId);
        }

        public static MapData GetMapData(int mapId)
        {
            string path = GetMapDataPath(mapId);
            StreamReader sr = new StreamReader(path);
            string json = sr.ReadLine();

            return JsonConvert.DeserializeObject<MapData>(json);
        }

        public static void SaveData<T>(string path, T data)
        {
            if (path == null || data == null)
            {
                return;
            }

            string json = JsonConvert.SerializeObject(data);
            FileInfo file = new FileInfo(path);
            StreamWriter sw = file.CreateText();
            sw.Write(json);
            sw.Close();
            sw.Dispose();
            AssetDatabase.Refresh();
        }
    }
}
