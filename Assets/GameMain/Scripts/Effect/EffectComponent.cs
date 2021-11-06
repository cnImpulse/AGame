using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using UnityEngine.Tilemaps;
using Cfg.Effect;

namespace SSRPG
{
    public class EffectComponent : GameFrameworkComponent
    {
        [SerializeField]
        private Transform m_EffectInstanceRoot = null;

        private Dictionary<int, Effect> m_LoadedEffectList = null;

        public Tilemap m_GridMapEffect = null;

        private void Start()
        {
            if (m_EffectInstanceRoot == null)
            {
                Log.Error("You must set effect instance root first.");
                return;
            }

            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnCreatEffect);

            m_GridMapEffect = GetComponentInChildren<Tilemap>();
            m_LoadedEffectList = new Dictionary<int, Effect>();
        }

        /// <summary>
        /// 创建特效
        /// </summary>
        /// <param name="type">特效类型</param>
        /// <param name="position">特效位置</param>
        /// <returns>特效实体Id</returns>
        public int CreatEffect(EffectType type, Vector3 position, float lifetime = -1)
        {
            int entityId = GameEntry.Entity.GenerateSerialId();
            EffectData effectData = new EffectData(entityId, (int)type, position, lifetime);

            GameEntry.Entity.ShowEffect(effectData);
            return entityId;
        }

        public Effect GetEffect(int entityId)
        {
            if (m_LoadedEffectList.TryGetValue(entityId, out var effect))
            {
                return effect;
            }

            return null;
        }

        public void ShowGridEffect(List<GridData> gridDatas, GridEffectType type, Color color = default)
        {
            List<Vector2Int> positions = gridDatas.ConvertAll((input) => input.GridPos);
            ShowGridEffect(positions, type, color);
        }

        public void ShowGridEffect(List<Vector2Int> positions, GridEffectType type, Color color = default)
        {
            m_GridMapEffect.ClearAllTiles();
            if (positions == null)
            {
                return;
            }

            if (color == default)
            {
                color = Color.white;
            }

            var cfg = GameEntry.Cfg.Tables.TblGridMapEffect.Get((int)type);
            string path = AssetUtl.GetTileAsset("Effect", cfg.AssetName);
            GameEntry.Resource.LoadAsset(path, typeof(TileBase),
                (assetName, asset, duration, userData) =>
                {
                    var tile = asset as TileBase;

                    m_GridMapEffect.color = color;
                    foreach (var pos in positions)
                    {
                        m_GridMapEffect.SetTile((Vector3Int)pos, tile);
                    }
                });
        }

        public void HideGridMapEffect()
        {
            m_GridMapEffect.ClearAllTiles();
        }

        /// <summary>
        /// 销毁特效
        /// </summary>
        /// <param name="entityId">特效实体Id</param>
        public void HideEffect(int entityId)
        {
            if (m_LoadedEffectList.ContainsKey(entityId))
            {
                m_LoadedEffectList.Remove(entityId);
                GameEntry.Entity.HideEntity(entityId);
            }
        }

        private void OnCreatEffect(object sender, GameFrameworkEventArgs e)
        {
            var ne = (ShowEntitySuccessEventArgs)e;
            if (ne.Entity.Logic is Entity)
            {
                m_LoadedEffectList.Add(ne.Entity.Id, ne.Entity.Logic as Effect);
                ne.Entity.transform.SetParent(m_EffectInstanceRoot);
            }
        }
    }
}