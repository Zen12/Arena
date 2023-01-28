using Lumpn.Pathfinding;
using UnityEngine;

namespace World
{
    public class Utils
    {
        public static Graph GetGraph(WorldModel model)
        {
            var size = model.Size;
            var graph = new Graph();

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    graph.AddNode(model.Nodes[i, j]);
                }
            }

            for (int i = 0; i < size.x - 1; i++)
            {
                for (int j = 0; j < size.y - 1; j++)
                {
                    var current = model.Nodes[i, j];
                    var right = model.Nodes[i + 1, j];
                    var down = model.Nodes[i, j + 1];


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