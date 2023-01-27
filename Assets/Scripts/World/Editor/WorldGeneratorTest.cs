using NUnit.Framework;
using UnityEngine;

namespace Editor
{
    public class WorldGeneratorTest
    {
        [Test]
        public void GIVEN_RANDOM__GENERATE__SHOULD_ALL_BE_1()
        {
            // Given
            var dummyRandom = new DummyRandom();
            dummyRandom.ReturnValue = 1;

            var worldGen = new WorldGenerator(dummyRandom);
            var size = new Vector2Int(10, 10);

            // Act
            var model = worldGen.GenerateRandom(size);
            
            // Should
            for (int i = 0; i <  size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    Assert.AreEqual(dummyRandom.ReturnValue, model.Nodes[i, j].Value);
                }
            }
        }
    }
}