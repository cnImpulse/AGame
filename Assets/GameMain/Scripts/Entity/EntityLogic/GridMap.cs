using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 网格地图。
    /// </summary>
    public class GridMap : Entity, IPointerDownHandler
    {
        [SerializeField]
        private GridMapData m_Data;

        [SerializeField]
        private List<GridUnit> m_GridUnitList = null;

        private Tile empty, wall;
        private Tilemap tilemap;
        private BoxCollider2D box;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            empty = AssetDatabase.LoadAssetAtPath<Tile>(AssetUtl.GetTileAsset("empty"));
            wall = AssetDatabase.LoadAssetAtPath<Tile>(AssetUtl.GetTileAsset("wall"));
            tilemap = transform.Find("Tilemap").GetComponent<Tilemap>();
            box = gameObject.GetOrAddComponent<BoxCollider2D>();

            m_GridUnitList = new List<GridUnit>();
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

            GridUnit gridUnit = childEntity as GridUnit;
            if (gridUnit == null)
            {
                return;
            }

            RegisterGridUnit(gridUnit);
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
            box.size = tilemap.localBounds.size;
        }

        public UnityEngine.Vector3 GridPosToWorldPos(Vector2Int gridPos)
        {
            return tilemap.GetCellCenterWorld((Vector3Int)gridPos);
        }

        /// <summary>
        /// 注册网格单位数据到地图
        /// </summary>
        /// <param name="gridUnit">网格单位</param>
        /// <returns>注册结果</returns>
        private bool RegisterGridUnit(GridUnit gridUnit)
        {
            int gridIndex = m_Data.GetGridIndex(gridUnit.Data.GridPos);
            GridData gridData = m_Data.GridList[gridIndex];
            if (gridData == null || gridData.GridType != GridType.Normal)
            {
                return false;
            }

            gridData.OnGridUnitEnter(gridUnit);
            Log.Info(gridData.GridPos);
            return true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Log.Info(eventData.position);
        }
    }
}
