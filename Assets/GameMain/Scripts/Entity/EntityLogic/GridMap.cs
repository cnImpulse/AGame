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
        private Dictionary<int, GridUnit> m_GridUnitList = null;

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

            m_GridUnitList = new Dictionary<int, GridUnit>();
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

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            foreach (var gridUnit in m_GridUnitList.Values)
            {
                GameEntry.Entity.HideEntity(gridUnit);
            }

            m_Tilemap.ClearAllTiles();
            HideTilemapEffect();
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

            GridUnit gridUnit = childEntity as GridUnit;
            if (gridUnit == null)
            {
                return;
            }

            UnRegisterGridUnit(gridUnit);
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

        public Vector3 GridPosToWorldPos(Vector2Int gridPos)
        {
            return m_Tilemap.GetCellCenterWorld((Vector3Int)gridPos);
        }

        /// <summary>
        /// 注册网格单位实体
        /// </summary>
        /// <param name="gridUnit">网格单位</param>
        /// <returns>注册结果</returns>
        private bool RegisterGridUnit(GridUnit gridUnit)
        {
            GridData gridData = m_Data.GetGridData(gridUnit.GridPos);
            if (gridData == null || gridData.GridType != GridType.Normal)
            {
                return false;
            }

            m_GridUnitList.Add(gridUnit.Id, gridUnit);
            gridData.OnGridUnitEnter(gridUnit);
            return true;
        }

        /// <summary>
        /// 注销网格单位实体
        /// </summary>
        /// <param name="gridUnit"></param>
        /// <returns></returns>
        public bool UnRegisterGridUnit(GridUnit gridUnit)
        {
            if (gridUnit == null || !m_GridUnitList.ContainsKey(gridUnit.Id))
            {
                return false;
            }

            m_GridUnitList.Remove(gridUnit.Id);
            gridUnit.GridData.OnGridUnitLeave();

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

        public void ShowMoveArea(List<GridData> gridDatas)
        {
            m_GridMapEffect.color = Color.yellow;
            ShowTilemapEffect(gridDatas, streak);
        }

        public void ShowAttackArea(List<GridData> gridDatas)
        {
            m_GridMapEffect.color = Color.red;
            ShowTilemapEffect(gridDatas, streak);
        }

        private void ShowTilemapEffect(List<GridData> gridDatas, TileBase tile)
        {
            if (gridDatas == null)
            {
                return;
            }

            m_GridMapEffect.ClearAllTiles();
            foreach (var grid in gridDatas)
            {
                m_GridMapEffect.SetTile((Vector3Int)grid.GridPos, tile);
            }
        }

        public void HideTilemapEffect()
        {
            m_GridMapEffect.ClearAllTiles();
        }

        public void MoveTo(GridUnit gridUnit, Vector2Int destination)
        {
            GridData start = m_Data.GetGridData(gridUnit.GridUnitData.GridPos);
            GridData end = m_Data.GetGridData(destination);

            start.OnGridUnitLeave();
            end.OnGridUnitEnter(gridUnit);

            gridUnit.GridUnitData.GridPos = destination;
        }

        public List<BattleUnit> GetBattleUnitList(CampType campType)
        {
            return GetGridUnitList<BattleUnit>(GridUnitType.BattleUnit, campType);
        }

        private List<T> GetGridUnitList<T>(GridUnitType gridUnitType, CampType campType)
            where T : GridUnit
        {
            List<T> gridUnitList = new List<T>();
            foreach (var gridUnit in m_GridUnitList.Values)
            {
                if (gridUnit.GridUnitType == gridUnitType && gridUnit.CampType == campType)
                {
                    gridUnitList.Add(gridUnit as T);
                }
            }

            return gridUnitList;
        }
    }
}
