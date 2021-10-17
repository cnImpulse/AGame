using System.IO;
using GameFramework;
using Newtonsoft.Json;

namespace SSRPG
{
    public static class AssetUtl
    {
        public static string GetEntityAssetPath(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Entities/{0}.prefab", assetName);
        }

        public static string GetEffectAssetPath(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Effect/{0}.prefab", assetName);
        }

        public static string GetTileAssetPath(string assetName)
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

        public static string GetSceneAssetPath(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Scenes/{0}.unity", assetName);
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
        }

        #region UI

        public static string GetUIFormAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/UI/UIForms/{0}.prefab", assetName);
        }

        #endregion
    }
}
