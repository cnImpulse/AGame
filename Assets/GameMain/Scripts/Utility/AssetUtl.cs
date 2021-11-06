using System.IO;
using GameFramework;
using UnityGameFramework.Runtime;
using GameFramework.Resource;
using Newtonsoft.Json;

namespace SSRPG
{
    public static class AssetUtl
    {
        public static string GetEntityAsset(string entityGroup, string name, int typeId)
        {
            string path = Utility.Text.Format("Assets/GameMain/Entities/{0}/{1}_{2}.prefab", entityGroup, name, typeId);
            if (GameEntry.Resource.HasAsset(path) == HasAssetResult.NotExist)
            {
                return Utility.Text.Format("Assets/GameMain/Entities/{0}/{1}.prefab", entityGroup, name);
            }

            return path;
        }

        public static string GetEffectAsset(string name)
        {
            return Utility.Text.Format("Assets/GameMain/Effect/{0}.prefab", name);
        }

        public static string GetTileAsset(string gridType, string name)
        {
            if (gridType == "")
            {
                return Utility.Text.Format("Assets/GameMain/Tilemap/Tiles/{0}.asset", name);
            }

            return Utility.Text.Format("Assets/GameMain/Tilemap/Tiles/{0}/{1}.asset", gridType, name);
        }

        public static string GetLevelData(int levelId)
        {
            return Utility.Text.Format("Assets/GameMain/GameData/LevelData/LevelData_{0}.json", levelId);
        }

        public static string GetSceneAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Scenes/{0}.unity", assetName);
        }

        public static void SaveData<T>(string path, T data)
        {
            string json = JsonConvert.SerializeObject(data);
            FileInfo file = new FileInfo(path);
            StreamWriter sw = file.CreateText();
            sw.Write(json);
            sw.Close();
            sw.Dispose();

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif

            UnityEngine.Debug.Log("保存数据成功!");
        }

        public static T ReadData<T>(string path)
        {
            StreamReader sr = new StreamReader(path);
            string json = sr.ReadLine();

            return Utility.Json.ToObject<T>(json);
        }

        public static string GetUIFormAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/UI/UIForms/{0}.prefab", assetName);
        }
    }
}
