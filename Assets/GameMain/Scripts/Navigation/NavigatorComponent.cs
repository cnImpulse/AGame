using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.ObjectPool;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class NavigatorComponent : GameFrameworkComponent
    {
        private class Node : IReference
        {
            public GridData gridData = null;
            public Node preNode = null;

            public static Node Create(GridData gridData, Node preNode = null)
            {
                Node node = ReferencePool.Acquire<Node>();
                node.gridData = gridData;
                node.preNode = preNode;
                return node;
            }

            public void Clear()
            {
                gridData = null;
                preNode = null;
            }
        }

        /// <summary>
        /// 获取到达目的地的路径,终点需要可到达,路径可通过,但是不一定能站立。例如队友占据的单位格可通过,但是不能站立。
        /// </summary>
        public bool Navigate(GridMapData mapData, BattleUnit battleUnit, GridData end, out List<GridData> path)
        {
            path = new List<GridData>();

            if (mapData == null || battleUnit == null || end == null)
            {
                return false;
            }

            if (!end.CanArrive())
            {
                Log.Warning("终点不可到达！");
                return false;
            }

            if (battleUnit.Data.GridPos == end.GridPos)
            {
                return true;
            }

            Queue<Node> open = new Queue<Node>();
            Dictionary<GridData, Node> close = new Dictionary<GridData, Node>();

            GridData start = mapData.GetGridData(battleUnit.Data.GridPos);
            open.Enqueue(Node.Create(start));

            int trytimes = 50;
            while (trytimes-- != 0)
            {
                int length = open.Count;
                if (length == 0)
                {
                    return false;
                }

                for (int j = 0; j < length; ++j)
                {
                    Node node = open.Dequeue();

                    if (node.gridData == end)
                    {
                        // 回溯
                        do
                        {
                            path.Add(node.gridData);
                            node = node.preNode;
                        } while (node != null && node.preNode != null);
                        path.Reverse();

                        return true;
                    }

                    List<GridData> neighbors = mapData.GetNeighbors(node.gridData.GridPos, battleUnit, NeighborType.CanAcross);
                    foreach (var neighbor in neighbors)
                    {
                        if (close.ContainsKey(neighbor) || Exit(open, neighbor))
                        {
                            continue;
                        }

                        open.Enqueue(Node.Create(neighbor, node));
                    }

                    close.Add(node.gridData, node);
                }
            }

            return false;
        }

        private bool Exit(Queue<Node> open, GridData gridData)
        {
            foreach (var node in open)
            {
                if (node.gridData == gridData)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
