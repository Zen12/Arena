using System.Collections.Generic;
using Database;
using UnityEngine;

namespace UnityView
{
    public class WorldView : MonoBehaviour, IPermutationListener
    {
        [SerializeField] private PrefabDatabase _database;
        private GameObject[,] _grid;
        private readonly List<UnitView> _units = new List<UnitView>();

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
        

        public void OnChangeModel(in PermutationChanges changes)
        {
            foreach (var unit in changes.Units)
            {
                if (unit.CommandType == CommandType.Idle)
                    continue;

                if (unit.CommandType == CommandType.Create)
                {
                    var prefab = _database.Team1Unit;
                    if (unit.TeamId == 1)
                    {
                        prefab = _database.Team2Unit;
                    }

                    var obj = GameObject.Instantiate(prefab);
                    var startPlatform = _grid[unit.Pos1.x, unit.Pos1.y];
                    var endPlatform = _grid[unit.Pos2.x, unit.Pos2.y];
                    var start = startPlatform.transform.position + Vector3.up;
                    var end = endPlatform.transform.position + Vector3.up;
                    obj.ApplyChange(start, end, unit.EndTime - unit.StartTime, CommandType.Create);
                    _units.Add(obj);
                }
            }
        }
        
    }
}
