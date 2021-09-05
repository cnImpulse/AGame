using GameFramework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 网格地图。
    /// </summary>
    public class GridMap : Entity
    {
        [SerializeField]
        private GridMapData m_Data;

        private Tile empty, wall;
        private Tilemap tilemap;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            empty = AssetDatabase.LoadAssetAtPath<Tile>(AssetUtl.GetTileAsset("empty"));
            wall = AssetDatabase.LoadAssetAtPath<Tile>(AssetUtl.GetTileAsset("wall"));
            tilemap = transform.Find("Tilemap").GetComponent<Tilemap>();

            gameObject.SetLayerRecursively(Constant.Layer.GridMapLayerId);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_Data = userData as GridMapData;
            if (m_Data == null)
            {
                Log.Error("GridMap object data is invalid.");
                return;
            }

            RefreshMap();
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
        }

        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
        }

        private void RefreshMap()
        {
            foreach(var gridData in m_Data.GridList.Values)
            {
                Tile tile = empty;
                if (gridData.GridType == GridType.Wall)
                {
                    tile = wall;
                }

                tilemap.SetTile((Vector3Int)gridData.GridPos, tile);
            }
        }

        public Vector3 GridPosToWorldPos(Vector2Int gridPos)
        {
            return tilemap.GetCellCenterWorld((Vector3Int)gridPos);
        }
    }
}
