using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 网格地图。
    /// </summary>
    public class GridMap : Entity, IPointerDownHandler
    {
        public static TileBase empty = null, wall = null, streak = null;

        private Tilemap m_Tilemap = null, m_GridMapEffect = null;
        private BoxCollider2D box = null;

        [SerializeField]
        private GridMapData m_Data = null;

        [SerializeField]
        private Dictionary<int, GridUnit> m_GridUnitList = null;

        public GridMapData Data => m_Data;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            gameObject.SetLayerRecursively(Constant.Layer.GridMapLayerId);

            m_Tilemap = transform.Find("Tilemap").GetComponent<Tilemap>();
            m_GridMapEffect = transform.Find("GridMapEffect").GetComponent<Tilemap>();
            box = gameObject.GetOrAddComponent<BoxCollider2D>();

            m_GridUnitList = new Dictionary<int, GridUnit>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_Data = userData as GridMapData;

            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntityScuess);

            GameEntry.Resource.LoadAsset(AssetUtl.GetTileAssetPath("empty"), typeof(TileBase),
                (assetName, asset, duration, userData) => { empty = asset as TileBase; });
            GameEntry.Resource.LoadAsset(AssetUtl.GetTileAssetPath("wall"), typeof(TileBase),
                (assetName, asset, duration, userData) => { wall = asset as TileBase; });
            GameEntry.Resource.LoadAsset(AssetUtl.GetTileAssetPath("streak"), typeof(TileBase),
                (assetName, asset, duration, userData) => { streak = asset as TileBase; RefreshMap(); });
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

            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntityScuess);
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);

            var gridUnit = childEntity as GridUnit;
            m_GridUnitList.Add(gridUnit.Id, gridUnit);
            Data.GetGridData(gridUnit.Data.GridPos).OnGridUnitEnter(gridUnit);
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

        public void SetGridData(Vector2Int position, GridType gridType)
        {
            var gridData = m_Data.GetGridData(position);
            gridData.GridType = gridType;
            RefreshGrid(gridData);
        }

        public void RefreshGrid(GridData gridData)
        {
            TileBase tile = empty;
            if (gridData.GridType == GridType.Wall)
            {
                tile = wall;
            }

            m_Tilemap.SetTile((Vector3Int)gridData.GridPos, tile);
        }

        public void RefreshMap()
        {
            m_Tilemap.ClearAllTiles();
            foreach(var gridData in m_Data.GridList.Values)
            {
                RefreshGrid(gridData);
            }
            box.size = m_Tilemap.localBounds.size;
        }

        public void RefreshPreview(List<GridData> gridList, GridType gridType)
        {
            RefreshMap();
            foreach (var gridData in gridList)
            {
                TileBase tile = empty;
                if (gridType == GridType.Wall)
                {
                    tile = wall;
                }
                m_Tilemap.SetTile((Vector3Int)gridData.GridPos, tile);
            }
        }

        public Vector3 GridPosToWorldPos(Vector2Int gridPos)
        {
            return m_Tilemap.GetCellCenterWorld((Vector3Int)gridPos);
        }

        public Vector2Int WorldPosToGridPos(Vector3 worldPosition)
        {
            return (Vector2Int)m_Tilemap.WorldToCell(worldPosition);
        }

        /// <summary>
        /// 注册战斗单位实体
        /// </summary>
        /// <param name="gridUnit">网格单位</param>
        /// <returns>注册结果</returns>
        public bool RegisterBattleUnit(BattleUnitData battleUnitData)
        {
            GridData gridData = m_Data.GetGridData(battleUnitData.GridPos);
            if (gridData == null || gridData.GridType != GridType.Normal)
            {
                Log.Warning("注册战斗单位失败!");
                return false;
            }

            GameEntry.Entity.ShowBattleUnit(battleUnitData);
            return true;
        }

        /// <summary>
        /// 注销网格单位实体
        /// </summary>
        private bool UnRegisterGridUnit(GridUnit gridUnit)
        {
            if (gridUnit == null || !m_GridUnitList.ContainsKey(gridUnit.Id))
            {
                return false;
            }

            m_GridUnitList.Remove(gridUnit.Id);
            gridUnit.GridData.OnGridUnitLeave();

            return true;
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
            GridData start = m_Data.GetGridData(gridUnit.Data.GridPos);
            GridData end = m_Data.GetGridData(destination);

            start.OnGridUnitLeave();
            end.OnGridUnitEnter(gridUnit);

            gridUnit.Data.GridPos = destination;
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
                if (gridUnit.Data.GridUnitType == gridUnitType && gridUnit.Data.CampType == campType)
                {
                    gridUnitList.Add(gridUnit as T);
                }
            }

            return gridUnitList;
        }

        private GridData GetGridDataByWorldPos(Vector3 worldPosition)
        {
            Vector2Int gridPos = (Vector2Int)m_Tilemap.WorldToCell(worldPosition);
            return m_Data.GetGridData(gridPos);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var gridData = GetGridDataByWorldPos(eventData.pointerCurrentRaycast.worldPosition);
            if (gridData != null)
            {
                GameEntry.Event.Fire(this, PointerDownGridMapEventArgs.Create(gridData));
            }
        }

        public void OnShowEntityScuess(object sender, GameEventArgs e)
        {
            var ne = (ShowEntitySuccessEventArgs)e;

            var gridUnit = ne.Entity.Logic as GridUnit;
            if (gridUnit == null || gridUnit.Data.ParentId != Id) 
            {
                return;
            }

            GameEntry.Entity.AttachEntity(gridUnit.Id, Id);
        }
    }
}
