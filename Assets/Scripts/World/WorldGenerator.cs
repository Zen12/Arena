using Lumpn.Pathfinding;
using UnityEngine;

public sealed class WorldGenerator
{
    private readonly IRandom _random;

    public WorldGenerator(IRandom random)
    {
        _random = random;
    }
    
    public WorldModel GenerateRandom(in Vector2Int size)
    {
        var graph = new Graph();
        var nodes = new Node[size.x, size.y];

        // create nodes
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                var r = _random.GetRandom(0, 3); // show how height of terrain
                var node = new Node(new Vector2Int(i, j), r);
                nodes[i, j] = node;
                node.Id = graph.AddNode(node);
            }
        }

        // connection
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

        return new WorldModel(size, graph, nodes);
    }
}

public interface IRandom
{
    public int GetRandom(in int start, in int end);
}


public sealed class Node : INode
{
    public int Id { get; internal set; }
    public readonly Vector2Int Position;
    public readonly int Value;

    public Node(Vector2Int position, int value)
    {
        Position = position;
        Value = value;
    }
}

public class WorldModel
{
    public readonly Vector2Int Size;
    public readonly Graph Graph;
    public readonly Node[,] Nodes;

    public WorldModel(in Vector2Int size, Graph graph, Node[,] nodes)
    {
        Size = size;
        Graph = graph;
        Nodes = nodes;
    }
}
