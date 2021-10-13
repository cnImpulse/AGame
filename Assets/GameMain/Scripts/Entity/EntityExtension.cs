using System;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public static class EntityExtension
    {
        // 关于 EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int s_SerialId = 0;

        public static T GetGameEntity<T>(this EntityComponent entityComponent, int entityId)
            where T : Entity
        {
            UnityGameFramework.Runtime.Entity entity = entityComponent.GetEntity(entityId);
            if (entity == null)
            {
                return null;
            }

            return entity.Logic as T;
        }

        public static void AttachEntity(this EntityComponent entityComponent, Entity entity, int parentId, string parentTransformPath = null, object userData = null)
        {
            entityComponent.AttachEntity(entity.Entity, parentId, parentTransformPath, userData);
        }

        public static void HideEntity(this EntityComponent entityComponent, Entity entity)
        {
            entityComponent.HideEntity(entity.Entity);
        }

        private static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup, EntityData data, int entityType)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            var cfg = GameEntry.Cfg.Tables.TblEntity.Get(entityType);
            entityComponent.ShowEntity(data.Id, logicType, AssetUtl.GetEntityAsset(cfg.AssetName), entityGroup, data);
        }

        public static void ShowGridMap(this EntityComponent entityComponent, GridMapData data)
        {
            entityComponent.ShowEntity(typeof(GridMap), "GridMap", data, 10000);
        }

        public static void ShowBattleUnit(this EntityComponent entityComponent, BattleUnitData data)
        {
            entityComponent.ShowEntity(typeof(BattleUnit), "BattleUnit", data, 20000);
        }

        public static void ShowEffect(this EntityComponent entityComponent, EffectDataBase data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            var cfg = GameEntry.Cfg.Tables.TblEffect.Get(data.TypeId);
            entityComponent.ShowEntity(data.Id, typeof(EffectBase), AssetUtl.GetEffectAsset(cfg.AssetName), "Effect", data);
        }

        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return --s_SerialId;
        }
    }
}
