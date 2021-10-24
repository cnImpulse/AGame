using System;
using UnityEngine;
using GameFramework;
using GameFramework.Resource;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public enum EntityType
    {
        GridMap = 10000,
        BattleUnit = 20000,
    }

    public static class EntityExtension
    {
        public static void ShowEffect(this EntityComponent entityComponent, EffectDataBase data)
        {
            var cfg = GameEntry.Cfg.Tables.TblEffect.Get(data.TypeId);
            entityComponent.ShowEntity(data.Id, typeof(EffectBase), AssetUtl.GetEffectAssetPath(cfg.AssetName), "Effect", data);
        }

        // 重构-----------------

        // 关于 EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int s_SerialId = 0;

        private static void ShowEntity<T>(this EntityComponent entityComponent, string entityGroup, EntityData data, int entityType)
            where T : EntityLogic
        {
            var cfg = GameEntry.Cfg.Tables.TblEntity.Get(entityType);
            entityComponent.ShowEntity<T>(data.Id, AssetUtl.GetEntityAssetPath(cfg.AssetName), entityGroup, cfg.Priority, data);
        }

        private static void ShowEntity<T>(this EntityComponent entityComponent, string entityGroup, EntityData data, EntityType entityType)
            where T : EntityLogic
        {
            entityComponent.ShowEntity<T>(entityGroup, data, (int)entityType);
        }

        public static T GetEntityLogic<T>(this EntityComponent entityComponent, int entityId)
            where T : EntityLogic
        {
            UnityGameFramework.Runtime.Entity entity = entityComponent.GetEntity(entityId);
            if (entity == null)
            {
                return null;
            }

            return entity.Logic as T;
        }

        public static void HideEntity(this EntityComponent entityComponent, Entity entity)
        {
            entityComponent.HideEntity(entity.Entity);
        }

        public static void AttachEntity(this EntityComponent entityComponent, Entity entity, int parentId, string parentTransformPath = null, object userData = null)
        {
            entityComponent.AttachEntity(entity.Entity, parentId, parentTransformPath, userData);
        }

        public static void ShowGridMap(this EntityComponent entityComponent, int mapId)
        {
            string path = AssetUtl.GetGridMapDataPath(mapId);
            GameEntry.Resource.LoadAsset(path, typeof(TextAsset), (assetName, asset, duration, userData) =>
            {
                TextAsset textAsset = asset as TextAsset;
                GridMapData gridMapData = Utility.Json.ToObject<GridMapData>(textAsset.text);
                entityComponent.ShowEntity<GridMap>("GridMap", gridMapData, EntityType.GridMap);
            });
        }

        public static void ShowGridMap(this EntityComponent entityComponent, GridMapData data)
        {
            entityComponent.ShowEntity<GridMap>("GridMap", data, EntityType.GridMap);
        }

        public static void ShowBattleUnit(this EntityComponent entityComponent, BattleUnitData data)
        {
            entityComponent.ShowEntity<BattleUnit>("BattleUnit", data, EntityType.BattleUnit);
        }

        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return --s_SerialId;
        }
    }
}
