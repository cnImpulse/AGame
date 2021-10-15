using System;
using GameFramework.Resource;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public static class ResourceExtension
    {
        public static void LoadAsset(this ResourceComponent resourceComponent, string assetName, LoadAssetSuccessCallback onLoadSuccess)
        {
            resourceComponent.LoadAsset(assetName, new LoadAssetCallbacks(onLoadSuccess));
        }

        public static void LoadAsset(this ResourceComponent resourceComponent, string assetName, Type type, LoadAssetSuccessCallback onLoadSuccess)
        {
            resourceComponent.LoadAsset(assetName, type, new LoadAssetCallbacks(onLoadSuccess));
        }
    }
}
