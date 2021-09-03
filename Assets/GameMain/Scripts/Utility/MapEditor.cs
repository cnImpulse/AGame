using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;

namespace SSRPG
{
    public class MapEditor : MonoBehaviour
    {
        private Tile empty, wall;
        private Tilemap tilemap = null;
        private MapData mapData = null;

        private void Awake()
        {
            empty = AssetDatabase.LoadAssetAtPath<Tile>(AssetUtl.GetTileAsset("empty"));
            wall = AssetDatabase.LoadAssetAtPath<Tile>(AssetUtl.GetTileAsset("wall"));
            tilemap = GetComponent<Tilemap>();
        }

        private void Start()
        {
            GetData();
            SaveData();
        }

        private void GetData()
        {
            BoundsInt bounds = tilemap.cellBounds;
            int width = bounds.size.x, height = bounds.size.y;

            mapData = new MapData(width, height);
            for (int i = bounds.xMin; i <= bounds.xMax; ++i)
            {
                for (int j = bounds.yMin; j <= bounds.yMax; ++j)
                {
                    Vector3Int pos = new Vector3Int(i, j, 0);
                    int gridIndex = GridMapUtl.GetGridIndex(width, (Vector2Int)pos);

                    Tile tile = tilemap.GetTile<Tile>(pos);
                    if (tile == empty)
                    {
                        mapData.GridList.Add(gridIndex, new GridData((Vector2Int)pos, GridType.Normal));
                    }
                    else if (tile == wall)
                    {
                        mapData.GridList.Add(gridIndex, new GridData((Vector2Int)pos, GridType.Wall));
                    }
                }
            }
        }

        private void SaveData()
        {
            if (mapData == null)
            {
                return;
            }

            Debug.Log("SaveStart!");

            string json = JsonConvert.SerializeObject(mapData);
            string path = AssetUtl.GetMapDataPath(1);
            FileInfo file = new FileInfo(path);
            StreamWriter sw = file.CreateText();
            sw.Write(json);
            sw.Close();
            sw.Dispose();
            AssetDatabase.Refresh();

            Debug.Log("Done!");
        }
    }
}

