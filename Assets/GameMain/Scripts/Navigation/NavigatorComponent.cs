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

        // 最终路径不包括起点,不包括终点,路径可通过,但是不一定能站立。例如队友占据的单位格可通过,但是不能站立。
        public bool Navigate(GridMapData mapData, BattleUnitData unitData, GridData end, out List<GridData> path)
        {
            path = new List<GridData>();

            if (mapData == null || unitData == null || end == null)
            {
                return false;
            }

            Queue<Node> open = new Queue<Node>();
            Dictionary<GridData, Node> close = new Dictionary<GridData, Node>();

            GridData start = mapData.GetGridData(unitData.GridPos);
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

                    var distance = node.gridData.GridPos - end.GridPos;
                    if (Mathf.Abs(distance.x) + Mathf.Abs(distance.y) <= 1)
                    {
                        // 回溯
                        while (node.preNode != null)
                        {
                            path.Add(node.gridData);
                            node = node.preNode;
                        }
                        path.Reverse();

                        return true;
                    }

                    List<GridData> neighbors = mapData.GetNeighbors(node.gridData.GridPos, unitData, NeighborType.CanAcross);
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
