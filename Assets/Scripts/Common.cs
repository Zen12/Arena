using Lumpn.Pathfinding;
using UnityEngine;

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