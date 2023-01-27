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

        public PermutationController(ITimeline timeline)
        {
            _timeline = timeline;
        }

        public void Add(IPermutationListener l) => _listeners.Add(l);
        public void Remove(IPermutationListener l) => _listeners.Remove(l);

        public void AddMoveUnit(in Vector2Int startPos, in Vector2Int endPos, float step)
        {
            _list.Add(new
                PermutationUnit(startPos, endPos,
                    _timeline.CurrentTime, _timeline.CurrentTime + step,
                    CommandType.Move));
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