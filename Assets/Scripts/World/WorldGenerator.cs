using Lumpn.Pathfinding;
using UnityEngine;
using UnityEngine.Assertions;

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
                var node = new Node(new Vector2Int(i, j), r,graph.nodeCount);
                nodes[i, j] = node;
                
                // it of node is generated only after you added to graph
                // in order to use readonly struct we double check if id from graph is the same
                Assert.AreEqual(graph.AddNode(node), node.Id);
            }
        }

        return new WorldModel(size, nodes);
    }
}

