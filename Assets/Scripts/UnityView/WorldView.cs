using Database;
using UnityEngine;

namespace UnityView
{
    public class WorldView : MonoBehaviour
    {
        [SerializeField] private PrefabDatabase _database;
        private GameObject[,] _grid;

        public void GenerateWorld(WorldModel model)
        {
            var prefab = _database.PrefabGridView;
            var size = model.Size;
            _grid = new GameObject[size.x, size.y];

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    var obj = GameObject.Instantiate(prefab);
                    _grid[i, j] = obj;
                    var node = model.Nodes[i, j];
                    obj.transform.position = new Vector3(i, node.Value * 0.1f, j);
                }
            }
        }
    }
}
