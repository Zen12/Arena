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

public interface IEngineView
{
    void ApplyChange(Vector3 start, Vector3 end, float deltaTime, CommandType commandType);

    void OnActivate();
    void OnDeactivate();
}


public interface IUnitPoolFabric
{
    IEngineView Get();
    void Set(IEngineView view);
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
    public readonly uint Id;
    public readonly uint TeamId;
    public readonly Vector2Int Pos1;
    public readonly Vector2Int Pos2;
    public readonly float StartTime;
    public readonly float EndTime;
    public readonly CommandType CommandType;

    public PermutationUnit(in Vector2Int pos1, in Vector2Int pos2, 
        in float startTime, in float endTime, 
        in CommandType commandType)
    {
        Id = uint.MaxValue;
        TeamId = uint.MaxValue;
        Pos1 = pos1;
        Pos2 = pos2;
        StartTime = startTime;
        EndTime = endTime;
        CommandType = commandType;
    }
    
    public PermutationUnit(in uint id,in uint team,
        in Vector2Int pos1, in Vector2Int pos2,
        in float startTime, in float endTime, 
        in CommandType commandType)
    {
        Id = id;
        TeamId = team;
        Pos1 = pos1;
        Pos2 = pos2;
        StartTime = startTime;
        EndTime = endTime;
        CommandType = commandType;
    }
}

public enum CommandType
{
    Idle, Move, Attack, Create
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

public readonly struct UnitModel
{
    public readonly uint Id;
    public readonly uint TeamId;
    public readonly uint Health;

    public UnitModel(uint id, uint teamId, uint health)
    {
        Id = id;
        TeamId = teamId;
        Health = health;
    }
}

[System.Serializable]
public readonly struct WorldModel
{
    public readonly Vector2Int Size;
    public readonly Node[,] Nodes;
    public readonly UnitModel[,] Units;

    public WorldModel(in Vector2Int size, Node[,] nodes, UnitModel[,] units)
    {
        Size = size;
        Nodes = nodes;
        Units = units;
    }
}