using System.Collections.Generic;
using System.Linq;
using Lumpn.Pathfinding;
using Permutation;
using UnityEngine;
using World;

namespace Simulation
{
    public sealed class BattleSimulation
    {
        private readonly ITimeline _time;
        private readonly PermutationController _permutation;
        private readonly WorldModel _model;
        private readonly Graph _originalGraph;
        private readonly AStarSearch _search;
        private readonly IRandom _random;

        private uint _currentSequenceId = 1;
        private float _unitMoveSpeed = 1f;
        private float _previousTime;

        private UnitModel[,] _units;

        public BattleSimulation(ITimeline time, PermutationController permutation, WorldModel model, IRandom random)
        {
            var size = model.Size;
            _time = time;
            _permutation = permutation;
            _model = model;
            _random = random;
            _originalGraph = Utils.GetGraph(model);

            _units = new UnitModel[size.x, size.y];
            
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j <  size.y; j++)
                {
                    _units[i, j] = new UnitModel(0, 0, 1);
                }
            }
            
            AddRandomUnit(1);
            AddRandomUnit(2);

            _search = new AStarSearch(_originalGraph.nodeCount);
            _previousTime = _time.CurrentTime;
            _permutation.Send();
        }

        private void AddRandomUnit(in uint teamId)
        {
            var x = _random.GetRandom(0, _model.Size.x);
            var y = _random.GetRandom(0, _model.Size.y);

            if (_units[x, y].TeamId != 0)
            {
                AddRandomUnit(teamId);
                return;
            }

            var hp = (uint)_random.GetRandom(2, 5);

            var p = new Vector2Int(x, y);
            
            var unit = new UnitModel(_currentSequenceId, teamId, hp);
            _permutation.AddNewUnit(p, _currentSequenceId, teamId);
            _units[x, y] = unit;
            _currentSequenceId++;
        }

        public BattleStatus NextMoves()
        {
            var delta = _time.CurrentTime - _previousTime;
            _previousTime = _time.CurrentTime;

            var size = _model.Size;

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    var unit = _units[i, j];
                    if (unit.TeamId != 0)
                    {
                        _ = AddUnitBehaviour(i, j, delta);
                    }
                }
            }


            foreach (var unit in _permutation.CurrentPermutation)
            {
                if (unit.CommandType == CommandType.Move)
                {
                    var old = _units[unit.Pos1.x, unit.Pos1.y];
                    _units[unit.Pos1.x, unit.Pos1.y] = new UnitModel(0, 0, 0);
                    _units[unit.Pos2.x, unit.Pos2.y] = old;
                }
            }
            
            _permutation.Send();


            return BattleStatus.Active;
        }

        private Vector2Int AddUnitBehaviour(in int x,in int y, in float deltaTime)
        {
            var unit = _units[x, y];
            var (closestPos, step) = FindClosestEnemy(x, y);
            
            if (step < 0)
                return new Vector2Int(-1, -1); // something is wrong!
            
            if (step <= 2) // attack
            {
                _permutation.AddAttackUnit(unit.Id, unit.TeamId, 
                    new Vector2Int(x,y), closestPos, deltaTime);
                return new Vector2Int(x, y);
            }
            else // move to it
            {
                var p1Node = _model.Nodes[x, y];
                var p2Node = _model.Nodes[closestPos.x, closestPos.y];
                var path = _search.FindPath(_originalGraph, p1Node.Id, p2Node.Id, NoHeuristic);

                var node = new Node(Vector2Int.down, 0, -1);

                var index = 0;
                foreach (var n in path)
                {
                    if (index == 1)
                    {
                        node = (Node)n;
                        break; // need second
                    }

                    index++;
                }
                
                if (index == 2) // at destination
                    return new Vector2Int(-1, -1);
                
                if (node.Id == -1) // not found path...
                    return new Vector2Int(-1, -1);

                var p = new Vector2Int(x, y);

                // preventing to move to same position
                if (_permutation.CurrentPermutation.Exists(_ => _.Pos2 == p || _.Pos1 == p))
                {
                    return new Vector2Int(-1, -1);
                }

                if (_units[node.Position.x, node.Position.y].TeamId != 0)
                {
                    return new Vector2Int(-1, -1);
                }
                
                
                _permutation.AddMoveUnit(unit.Id, unit.TeamId, 
                    p, node.Position, deltaTime);

                return node.Position;
            }
        }

        private (Vector2Int, int) FindClosestEnemy(in int x,in int y)
        {
            var maxSize = _model.Size;
            var currentSize = 1;
            var startTeamId = _units[x, y].TeamId;
            while (true)
            {
                for (int i = x - currentSize; i < x + currentSize; i++)
                {
                    if (i < 0)
                        continue;
                    if (i >= maxSize.x)
                        continue;

                    for (int j = y - currentSize; j < y + currentSize; j++)
                    {
                        if (j < 0)
                            continue;
                        if (j >= maxSize.y)
                            continue;

                        var teamId = _units[i, j].TeamId;
                        if (teamId == 0 || teamId == startTeamId)
                            continue;

                        return (new Vector2Int(i, j), currentSize);
                    }
                }

                currentSize++;
                if (currentSize > maxSize.x && currentSize > maxSize.y)
                    break;
            }

            return (Vector2Int.zero, -1);
        }
        
        
        private static float NoHeuristic(IGraph graph, INode a, INode b)
        {
            return 0f;
        }
        
    }

    public enum BattleStatus
    {
        Active, Finish
    }
    
}