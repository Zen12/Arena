using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Permutation
{
    public sealed class PermutationController
    {
        private readonly ITimeline _timeline;
        
        private readonly List<IPermutationListener> _listeners = new List<IPermutationListener>();
        private readonly List<PermutationUnit> _list = new List<PermutationUnit>();
        public List<PermutationUnit> CurrentPermutation => _list;

        public PermutationController(ITimeline timeline)
        {
            _timeline = timeline;
        }

        public void Add(IPermutationListener l) => _listeners.Add(l);
        public void Remove(IPermutationListener l) => _listeners.Remove(l);
        
        public void AddTakeDamageUnit(in uint id, in uint teamId, in Vector2Int startPos, in Vector2Int endPos, float step)
        {
            _list.Add(new
                PermutationUnit(id, teamId, startPos, endPos,
                    _timeline.CurrentTime, _timeline.CurrentTime + step,
                    CommandType.TakeDamage));
        }
        
        public void AddDieUnit(in uint id, in uint teamId, in Vector2Int startPos, in Vector2Int endPos, float step)
        {
            _list.Add(new
                PermutationUnit(id, teamId, startPos, endPos,
                    _timeline.CurrentTime, _timeline.CurrentTime + step,
                    CommandType.Die));
        }
        
        public void AddAttackUnit(in uint id, in uint teamId, in Vector2Int startPos, in Vector2Int endPos, float step)
        {
            _list.Add(new
                PermutationUnit(id, teamId, startPos, endPos,
                    _timeline.CurrentTime, _timeline.CurrentTime + step,
                    CommandType.Attack));
        }

        public void AddMoveUnit(in uint id, in uint teamId, in Vector2Int startPos, in Vector2Int endPos, float step)
        {
            _list.Add(new
                PermutationUnit(id, teamId, startPos, endPos,
                    _timeline.CurrentTime, _timeline.CurrentTime + step,
                    CommandType.Move));
        }
        
        public void AddNewUnit(in Vector2Int pos, in uint id, in uint teamId)
        {
            _list.Add(new
                PermutationUnit( id, teamId, pos, pos,
                    _timeline.CurrentTime, _timeline.CurrentTime,
                    CommandType.Create));
        }

        public void Send()
        {
            var allPermutation = new PermutationChanges(_list.ToArray());
            _list.Clear();

            foreach (var listener in _listeners)
            {
                listener.OnChangeModel(allPermutation);
            }
        }
    }
}