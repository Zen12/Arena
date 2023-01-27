using Lumpn.Pathfinding;
using UnityEngine;

public interface IRandom
{
    public int GetRandom(in int start, in int end);
}
public interface ITimeline
{
    float CurrentTime { get; }
}

public readonly struct Unit
{
    public readonly uint Id;
    public readonly uint TeamId;
}

public interface IPermutationListener
{
    void OnChangeModel(in PermutationChanges changes);
}

[System.Serializable]
public readonly struct PermutationChanges
{
    public readonly PermutationUnit[] Units;

    public PermutationChanges(in PermutationUnit[] units)
    {
        Units = units;
    }
}

[System.Serializable]
public readonly struct PermutationUnit
{
    public readonly Vector2Int Pos1;
    public readonly Vector2Int Pos2;
    public readonly float StartTime;
    public readonly float EndTime;
    public readonly CommandType CommandType;

    public PermutationUnit(Vector2Int pos1, Vector2Int pos2, float startTime, float endTime, CommandType commandType)
    {
        Pos1 = pos1;
        Pos2 = pos2;
        StartTime = startTime;
        EndTime = endTime;
        CommandType = commandType;
    }
}

public enum CommandType
{
    Idle, Move, Attack
}


public struct Node : INode
{
    public int Id { get; internal set; }
    public readonly Vector2Int Position;
    public readonly int Value;

    public Node(Vector2Int position, int value)
    {
        Position = position;
        Value = value;
        Id = default;
    }
}

[System.Serializable]
public readonly struct WorldModel
{
    public readonly float Time;
    public readonly Vector2Int Size;
    public readonly Node[,] Nodes;
    public readonly uint[,] Units;

    public WorldModel(in Vector2Int size, Node[,] nodes, uint[,] units, float time)
    {
        Size = size;
        Nodes = nodes;
        Time = time;
        Units = units;
    }
}