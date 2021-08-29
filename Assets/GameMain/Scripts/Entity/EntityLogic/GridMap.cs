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

        private Tile empty;
        private Tilemap tilemap;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            empty = AssetDatabase.LoadAssetAtPath<Tile>(AssetUtility.GetTileAsset("empty"));
            tilemap = transform.Find("Tilemap").GetComponent<Tilemap>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_Data = userData as GridMapData;
            Name = Utility.Text.Format("[GridMap {0}]", Id);
            Camera.main.transform.position = new Vector3(m_Data.Width / 2f, m_Data.Height / 2f, -10);

            RefreshMap();
        }

        private void RefreshMap()
        {
            if (m_Data == null || tilemap == null || empty == null) 
            {
                return;
            }

            for(int i=0; i < m_Data.Height; ++i)
            {
                for(int j=0; j< m_Data.Width; ++j)
                {
                    tilemap.SetTile(new Vector3Int(j, i, 0), empty);
                }
            }
        }
    }
}
