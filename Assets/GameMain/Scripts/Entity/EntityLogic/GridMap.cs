using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using GameFramework.Resource;
using Cfg.Effect;

namespace SSRPG
{
    /// <summary>
    /// 网格地图。
    /// </summary>
    public class GridMap : Entity, IPointerDownHandler
    {
        private Tilemap[] m_TilemapList = null;

        [SerializeField]
        private GridMapData m_Data = null;

        [SerializeField]
        private Dictionary<int, GridUnit> m_GridUnitList = null;

        public GridMapData Data => m_Data;

        private void InitGridMapData(object userData)
        {
            m_Data = userData as GridMapData;
            if (m_Data == null)
            {
                Log.Warning("网格地图数据初始化错误。");
                return;
            }

            for (int i = m_TilemapList.Length - 1; i >= 0; --i)
            {
                var tilemap = m_TilemapList[i];
                var bounds = tilemap.cellBounds;
                for (int x = bounds.xMin; x <= bounds.xMax; ++x)
                {
                    for (int y = bounds.yMin; y <= bounds.yMax; ++y)
                    {
                        var position = new Vector3Int(x, y, 0);
                        if (m_Data.GetGridData((Vector2Int)position) != null)
                        {
                            continue;
                        }

                        var tile = tilemap.GetTile<TileBase>(position);
                        if (tile == null)
                        {
                            continue;
                        }

                        var gridData = CreatGridData((Vector2Int)position, tile);
                        m_Data.SetGridData(gridData);
                    }
                }
            }
        }

        private GridData CreatGridData(Vector2Int position, TileBase tile)
        {
            var obstaclePath = AssetUtl.GetTileAsset(GridType.Obstacle.ToString(), tile.name);
            if (GameEntry.Resource.HasAsset(obstaclePath) != HasAssetResult.NotExist)
            {
                return new GridData(position, GridType.Obstacle);
            }

            return new GridData(position, GridType.Land);
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            InitLayer("GridMap");

            m_TilemapList = GetComponentsInChildren<Tilemap>();
            m_TilemapList[0].gameObject.GetOrAddComponent<BoxCollider2D>();

            m_GridUnitList = new Dictionary<int, GridUnit>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowBattleUnitScuess);

            InitGridMapData(userData);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowBattleUnitScuess);

            base.OnHide(isShutdown, userData);
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

            var gridUnit = childEntity as GridUnit;
            m_GridUnitList.Remove(gridUnit.Id);
            Data.GetGridData(gridUnit.Data.GridPos).OnGridUnitLeave();
        }

        /// <summary>
        /// 网格坐标转世界坐标
        /// </summary>
        public Vector3 GridPosToWorldPos(Vector2Int gridPos)
        {
            return m_TilemapList[0].GetCellCenterWorld((Vector3Int)gridPos);
        }

        /// <summary>
        /// 世界坐标转网格坐标
        /// </summary>
        public Vector2Int WorldPosToGridPos(Vector3 worldPosition)
        {
            return (Vector2Int)m_TilemapList[0].WorldToCell(worldPosition);
        }

        /// <summary>
        /// 注册战斗单位实体
        /// </summary>
        public bool RegisterBattleUnit(RoleData roleData, Vector2Int gridPos, CampType campType)
        {
            BattleUnitData battleUnitData = new BattleUnitData(roleData, gridPos, campType);
            GridData gridData = m_Data.GetGridData(battleUnitData.GridPos);
            if (gridData == null || !gridData.CanArrive())
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
        public bool UnRegisterGridUnit(GridUnit gridUnit)
        {
            if (gridUnit == null || !m_GridUnitList.ContainsKey(gridUnit.Id))
            {
                Log.Warning("注销网格单位失败!");
                return false;
            }

            GameEntry.Entity.HideEntity(gridUnit);
            return true;
        }

        public void ShowMoveArea(List<GridData> gridDatas)
        {
            GameEntry.Effect.ShowGridEffect(gridDatas, GridEffectType.Streak, Color.yellow);
        }

        public void ShowAttackArea(List<GridData> gridDatas)
        {
            GameEntry.Effect.ShowGridEffect(gridDatas, GridEffectType.Streak, Color.red);
        }

        public void MoveTo(GridUnit gridUnit, Vector2Int destination)
        {
            GridData start = m_Data.GetGridData(gridUnit.Data.GridPos);
            GridData end = m_Data.GetGridData(destination);

            start.OnGridUnitLeave();
            end.OnGridUnitEnter(gridUnit);

            gridUnit.Data.GridPos = destination;
        }

        public List<T> GetGridUnitList<T>(CampType campType = CampType.None)
            where T : GridUnit
        {
            List<T> gridUnitList = new List<T>();
            foreach (var gridUnit in m_GridUnitList.Values)
            {
                if (gridUnit is T && (campType == CampType.None || gridUnit.Data.CampType == campType))
                {
                    gridUnitList.Add(gridUnit as T);
                }
            }

            return gridUnitList;
        }

        private GridData GetGridDataByWorldPos(Vector3 worldPosition)
        {
            Vector2Int gridPos = (Vector2Int)m_TilemapList[0].WorldToCell(worldPosition);
            return m_Data.GetGridData(gridPos);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var gridData = GetGridDataByWorldPos(eventData.pointerCurrentRaycast.worldPosition);
            if (gridData != null)
            {
                GameEntry.Event.Fire(this, EventName.PointerDownGridMap, gridData);
            }
        }

        public void OnShowBattleUnitScuess(object sender, GameEventArgs e)
        {
            var ne = (ShowEntitySuccessEventArgs)e;

            var gridUnit = ne.Entity.Logic as GridUnit;
            if (gridUnit == null) 
            {
                return;
            }

            GameEntry.Entity.AttachEntity(gridUnit.Id, Id);
        }
    }
}
