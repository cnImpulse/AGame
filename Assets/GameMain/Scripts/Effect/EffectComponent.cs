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

        [SerializeField]
        private Dictionary<int, Effect> m_EffectList = null;

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
            m_EffectList = new Dictionary<int, Effect>();
        }

        private void Update()
        {
            
        }

        /// <summary>
        /// 创建特效
        /// </summary>
        /// <param name="type">特效类型</param>
        /// <param name="position">特效位置</param>
        /// <returns>特效实体Id</returns>
        public int CreatEffect(EffectType type, Vector3 position)
        {
            int entityId = GameEntry.Entity.GenerateSerialId();
            EffectData effectData = new EffectData(entityId, (int)type, position);

            GameEntry.Entity.ShowEffect(effectData);
            return entityId;
        }

        public void ShowGridMapEffect(List<GridData> gridDatas, GridEffectType type, Color color = default)
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

            var cfg = GameEntry.Cfg.Tables.TblGridMapEffect.Get(type);
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
        public void DestoryEffect(int entityId)
        {
            if (entityId == 0)
            {
                return;
            }
            GameEntry.Entity.HideEntity(entityId);
        }

        /// <summary>
        /// 改变特效位置
        /// </summary>
        /// <param name="entityId">特效实体Id</param>
        /// <param name="position">特效位置</param>
        public bool ChangeEffectPos(int entityId, Vector3 position)
        {
            if (m_EffectList.TryGetValue(entityId, out var effect))
            {
                effect.transform.position = position;
                return true;
            }
            return false;
        }

        private void OnCreatEffect(object sender, GameFrameworkEventArgs e)
        {
            var ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType != typeof(Effect))
            {
                return;
            }

            m_EffectList.Add(ne.Entity.Id, ne.Entity.Logic as Effect);
            ne.Entity.transform.SetParent(m_EffectInstanceRoot);
        }
    }
}