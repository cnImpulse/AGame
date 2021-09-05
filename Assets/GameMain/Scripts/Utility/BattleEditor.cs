using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;

namespace SSRPG
{
    public class BattleEditor : MonoBehaviour
    {
        private Tile enemy_1, player;
        private Tilemap tilemap = null;
        private BattleData battleData = new BattleData();

        private void Awake()
        {
            enemy_1 = AssetDatabase.LoadAssetAtPath<Tile>(AssetUtl.GetTileAsset("enemy_1"));
            player = AssetDatabase.LoadAssetAtPath<Tile>(AssetUtl.GetTileAsset("player"));
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

            battleData.mapId = 1;
            battleData.maxPlayerBattleUnit = 2;
            for (int i = bounds.xMin; i <= bounds.xMax; ++i)
            {
                for (int j = bounds.yMin; j <= bounds.yMax; ++j)
                {
                    Vector3Int pos = new Vector3Int(i, j, 0);
                    int gridIndex = GridMapUtl.GetGridIndex(width, (Vector2Int)pos);

                    Tile tile = tilemap.GetTile<Tile>(pos);
                    if (tile == enemy_1)
                    {
                        battleData.enemyIds.Add(20000);
                        battleData.enemyPos.Add((Vector2Int)pos);
                    }
                    else if (tile == player)
                    {
                        battleData.playerBrithPos.Add((Vector2Int)pos);
                    }
                }
            }
        }

        private void SaveData()
        {
            Debug.Log("SaveStart!");

            string path = AssetUtl.GetBattleDataPath(1);
            AssetUtl.SaveData(path, battleData);

            Debug.Log("Done!");
        }
    }
}

