using UnityEngine;
using UnityEngine.Tilemaps;

namespace SSRPG
{
    [RequireComponent(typeof(Tilemap))]
    public class LevelEditor : MonoBehaviour
    {
        [SerializeField]
        [InspectorName("关卡Id")]
        private int m_LevelId = 0;

        [SerializeField]
        [InspectorName("地图Id")]
        private int m_MapId = 0;

        [SerializeField]
        [InspectorName("玩家战斗单位上限")]
        private int m_MaxPlayerBattleUnit = 3;

        private LevelEditor() { }

        [ContextMenu("保存数据")]
        private void SaveData()
        {
            var data = new LevelData(m_MapId);
            data.maxPlayerBattleUnit = m_MaxPlayerBattleUnit;

            var tilemap = GetComponent<Tilemap>();
            var bounds = tilemap.cellBounds;
            for (int x = bounds.xMin; x <= bounds.xMax; ++x)
            {
                for (int y = bounds.yMin; y <= bounds.yMax; ++y)
                {
                    var position = new Vector3Int(x, y, 0);
                    var tile = tilemap.GetTile<TileBase>(position);
                    if (tile == null)
                    {
                        continue;
                    }

                    if (tile.name == "brith")
                    {
                        data.playerBrithList.Add((Vector2Int)position);
                    }
                    else 
                    {
                        var typeId = 0;
                        if (tile.name != "default")
                        {
                            typeId = int.Parse(tile.name);
                        }

                        var index = GridMapUtl.GridPosToIndex((Vector2Int)position);
                        data.enemyList.Add(index, typeId);
                    }
                }
            }

            string path = AssetUtl.GetLevelData(m_LevelId);
            AssetUtl.SaveData(path, data);
        }
    }
}
