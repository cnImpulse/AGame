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
        // 重构-----------------

        // 关于 EntityId 的约定：
        // 0 为无效
        private static int s_SerialId = 0;

        private static void ShowEntity<T>(this EntityComponent entityComponent, EntityData data, EntityType entityType)
            where T : EntityLogic
        {
            var cfg = GameEntry.Cfg.Tables.TblEntity.Get((int)entityType);
            var path = AssetUtl.GetEntityAsset(cfg.Group, cfg.AssetName, data.TypeId);
            entityComponent.ShowEntity<T>(data.Id, path, cfg.Group, cfg.Priority, data);
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
            GridMapData data = new GridMapData(mapId);
            entityComponent.ShowEntity<GridMap>(data, EntityType.GridMap);
        }

        public static void ShowBattleUnit(this EntityComponent entityComponent, BattleUnitData data)
        {
            entityComponent.ShowEntity<BattleUnit>(data, EntityType.BattleUnit);
        }

        public static void ShowEffect(this EntityComponent entityComponent, EffectDataBase data)
        {
            var cfg = GameEntry.Cfg.Tables.TblEffect.Get(data.TypeId);
            entityComponent.ShowEntity(data.Id, typeof(EffectBase), AssetUtl.GetEffectAsset(cfg.AssetName), "Effect", data);
        }

        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return s_SerialId++;
        }
    }
}
