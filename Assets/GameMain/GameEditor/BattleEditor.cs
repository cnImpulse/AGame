using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace SSRPG
{
    public class BattleEditor : MonoBehaviour
    {
        private Tile enemy_1, player;
        private Tilemap tilemap = null;
        private BattleData battleData = new BattleData();

        [SerializeField]
        private int mapId = 1;

        [SerializeField]
        private int battleId = 1;

        [SerializeField]
        private int maxPlayerBattleUnit = 3;

        private void Awake()
        {
            enemy_1 = AssetDatabase.LoadAssetAtPath<Tile>(AssetUtl.GetTileAssetPath("enemy_1"));
            player = AssetDatabase.LoadAssetAtPath<Tile>(AssetUtl.GetTileAssetPath("player"));
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

            battleData.MapId = mapId;
            battleData.MaxPlayerBattleUnit = maxPlayerBattleUnit;
            for (int i = bounds.xMin; i <= bounds.xMax; ++i)
            {
                for (int j = bounds.yMin; j <= bounds.yMax; ++j)
                {
                    Vector3Int pos = new Vector3Int(i, j, 0);
                    int gridIndex = GridMapUtl.GetGridIndex(width, (Vector2Int)pos);

                    Tile tile = tilemap.GetTile<Tile>(pos);
                    if (tile == enemy_1)
                    {
                        battleData.EnemyIdList.Add(10000);
                        battleData.EnemyPosList.Add((Vector2Int)pos);
                    }
                    else if (tile == player)
                    {
                        battleData.PlayerBrithPos.Add((Vector2Int)pos);
                    }
                }
            }
        }

        private void SaveData()
        {
            Debug.Log("SaveStart!");

            string path = AssetUtl.GetBattleDataPath(battleId);
            AssetUtl.SaveData(path, battleData);

            Debug.Log("Done!");
        }
    }
}

