using Lumpn.Pathfinding;
using UnityEngine;

namespace World
{
    public class Utils
    {
        public static Graph GetGraph(WorldModel model)
        {
            var size = model.Size;
            var nodes = model.Nodes;
            var graph = new Graph();
            for (int i = 0; i < size.x - 1; i+=2)
            {
                for (int j = 0; j < size.y - 1; j+=2)
                {
                    var current = nodes[i, j];
                    var right = nodes[i + 1, j];
                    var down = nodes[i, j + 1];

                    var price = Mathf.Abs(current.Value - right.Value);
                    graph.AddEdge(current.Id, right.Id, price);
                    graph.AddEdge(right.Id, current.Id, price);
                
                
                    price = Mathf.Abs(current.Value - down.Value);
                    graph.AddEdge(current.Id, down.Id, price);
                    graph.AddEdge(down.Id, current.Id, price);
                }
            }

            return graph;
        }
    }
}