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

        return new WorldModel(size, nodes, new UnitModel[size.x, size.y]);
    }
}

