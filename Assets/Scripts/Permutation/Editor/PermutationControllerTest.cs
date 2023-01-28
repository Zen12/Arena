using NUnit.Framework;
using UnityEngine;

namespace Permutation.Editor
{
    public class PermutationControllerTest
    {
        [Test]
        public void GIVEN_PERMUTATION__CHANGE_UNIT__SHOULD_CALL_LISTENERS()
        {
            // Given
            var listener = new DummyListener();
            var time = new DummyTimeline();
            var per = new PermutationController(time);
            per.Add(listener);

            // Act
            per.AddMoveUnit(1, 1, Vector2Int.one, Vector2Int.up, 1f);
            per.Send();
            
            // Should
            Assert.AreEqual(1, listener.Last.Units.Length);
            Assert.AreEqual(Vector2Int.one, listener.Last.Units[0].Pos1);
            Assert.AreEqual(Vector2Int.up, listener.Last.Units[0].Pos2);
            Assert.AreEqual(CommandType.Move, listener.Last.Units[0].CommandType);
            Assert.AreEqual(time.CurrentTime, listener.Last.Units[0].StartTime);
            Assert.AreEqual(time.CurrentTime + 1f, listener.Last.Units[0].EndTime, 0.1f);
            
        }
    }
}