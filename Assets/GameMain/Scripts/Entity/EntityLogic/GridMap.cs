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

        private Tile empty;
        private Tilemap tilemap;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            tilemap = transform.Find("Tilemap").GetComponent<Tilemap>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_Data = userData as GridMapData;
            RefreshMap();
        }

        private void RefreshMap()
        {
            if (m_Data == null || tilemap == null)
            {
                return;
            }
        }
    }
}
