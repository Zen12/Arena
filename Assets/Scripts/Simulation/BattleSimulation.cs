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
        private readonly Graph _graph;
        private readonly AStarSearch _search;
        private readonly IRandom _random;

        private uint _currentSequenceId = 1;
        private float _unitMoveSpeed = 1f;
        private float _previousTime;
        private SimulationArena _arena = new SimulationArena();

        public BattleSimulation(ITimeline time, PermutationController permutation, WorldModel model, IRandom random)
        {
            _time = time;
            _permutation = permutation;
            _model = model;
            _random = random;
            _graph = Utils.GetGraph(model);
            
            (_arena.Team1,_arena.Team1Pos) = AddRandomUnit(0);
            (_arena.Team2,_arena.Team2Pos) = AddRandomUnit(1);

            _search = new AStarSearch(_graph.nodeCount);
            _previousTime = _time.CurrentTime;
            _permutation.Send();
        }

        private (UnitModel, Vector2Int) AddRandomUnit(in uint teamId)
        {
            var x = _random.GetRandom(0, _model.Size.x);
            var y = _random.GetRandom(0, _model.Size.y);

            // prevents putting same unit on same place
            if (_model.Units[x, y].Id != 0)
            {
                return AddRandomUnit(teamId);
            }

            var hp = (uint)_random.GetRandom(2, 5);

            var p = new Vector2Int(x, y);
            

            _model.Units[x, y] = new UnitModel(_currentSequenceId, teamId, hp);
            _permutation.AddNewUnit(p, _currentSequenceId, teamId);
            _currentSequenceId++;
            return (_model.Units[x, y], p);
        }

        public BattleStatus NextMoves()
        {
            var delta = _time.CurrentTime - _previousTime;
            
            var p1Node = _model.Nodes[_arena.Team1Pos.x, _arena.Team2Pos.y];
            var p2Node = _model.Nodes[_arena.Team2Pos.x, _arena.Team2Pos.y];
            var path1 = _search.FindPath(_graph, p1Node.Id, p2Node.Id, NoHeuristic);
            var path2 = _search.FindPath(_graph, p2Node.Id, p1Node.Id, NoHeuristic);

            var nextNode1 = (Node) path1.First();
            var nextNode2 = (Node) path2.First();

            // doesn't reach each other
            if (nextNode1.Id != p2Node.Id)
            {
                {
                    var old = _model.Units[_arena.Team2Pos.x, _arena.Team2Pos.y];
                    _model.Units[_arena.Team2Pos.x, _arena.Team2Pos.y] = new UnitModel();
                    _model.Units[nextNode2.Position.x, nextNode2.Position.y] = old;
                    _permutation.AddMoveUnit(_arena.Team2Pos, nextNode2.Position, delta);
                    _arena.Team2Pos = nextNode2.Position;
                }
                
                {
                    var old = _model.Units[_arena.Team1Pos.x, _arena.Team1Pos.y];
                    _model.Units[_arena.Team1Pos.x, _arena.Team1Pos.y] = new UnitModel();
                    _model.Units[nextNode1.Position.x, nextNode1.Position.y] = old;
                    _permutation.AddMoveUnit(_arena.Team1Pos, nextNode1.Position, delta);
                    _arena.Team1Pos = nextNode1.Position;
                }
                _permutation.Send();
            }

            return BattleStatus.Active;
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

    public class SimulationArena
    {
        public UnitModel Team1;
        public Vector2Int Team1Pos;
        public UnitModel Team2;
        public Vector2Int Team2Pos;
    }
}