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
        private GridMapData m_Data = null;

        [SerializeField]
        private List<GridUnit> m_GridUnitList = null;

        private TileBase empty, wall, streak;
        private Tilemap m_Tilemap, m_GridMapEffect;
        private BoxCollider2D box;

        public GridMapData GridMapData
        {
            get
            {
                return m_Data;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            empty = AssetDatabase.LoadAssetAtPath<Tile>(AssetUtl.GetTileAsset("empty"));
            wall = AssetDatabase.LoadAssetAtPath<Tile>(AssetUtl.GetTileAsset("wall"));
            streak = AssetDatabase.LoadAssetAtPath<TileBase>(AssetUtl.GetTileAsset("streak"));
            m_Tilemap = transform.Find("Tilemap").GetComponent<Tilemap>();
            m_GridMapEffect = transform.Find("GridMapEffect").GetComponent<Tilemap>();
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
                TileBase tile = empty;
                if (gridData.GridType == GridType.Wall)
                {
                    tile = wall;
                }

                m_Tilemap.SetTile((Vector3Int)gridData.GridPos, tile);
            }
            box.size = m_Tilemap.localBounds.size;
        }

        public UnityEngine.Vector3 GridPosToWorldPos(Vector2Int gridPos)
        {
            return m_Tilemap.GetCellCenterWorld((Vector3Int)gridPos);
        }

        /// <summary>
        /// 注册网格单位数据到地图
        /// </summary>
        /// <param name="gridUnit">网格单位</param>
        /// <returns>注册结果</returns>
        private bool RegisterGridUnit(GridUnit gridUnit)
        {
            int gridIndex = m_Data.GetGridIndex(gridUnit.GridData.GridPos);
            GridData gridData = m_Data.GridList[gridIndex];
            if (gridData == null || gridData.GridType != GridType.Normal)
            {
                return false;
            }

            gridData.OnGridUnitEnter(gridUnit);
            return true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Vector2Int gridPos = (Vector2Int)m_Tilemap.WorldToCell(eventData.pointerPressRaycast.worldPosition);
            GridData gridData = m_Data.GetGridData(gridPos);
            if (gridData != null)
            {
                GameEntry.Event.Fire(this, PointGridMapEventArgs.Create(gridData));
            }
        }

        public void ShowCanMoveArea(List<GridData> gridDatas)
        {
            if (gridDatas == null)
            {
                return;
            }

            m_GridMapEffect.ClearAllTiles();
            foreach (var grid in gridDatas)
            {
                m_GridMapEffect.SetTile((Vector3Int)grid.GridPos, streak);
            }
        }

        public void HideCanMoveArea()
        {
            m_GridMapEffect.ClearAllTiles();
        }

        public void MoveTo(GridUnit gridUnit, Vector2Int destination)
        {
            GridData gridData = m_Data.GetGridData(destination);

            gridData.OnGridUnitLeave();
            gridData.OnGridUnitEnter(gridUnit);
        }
    }
}
