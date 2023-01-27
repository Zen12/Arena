using UnityEngine;
using UnityView;

namespace Database
{
    [CreateAssetMenu(fileName = "PrefabDatabase", menuName = "_Config/PrefabDatabase", order = 1)]
    public sealed class PrefabDatabase : ScriptableObject
    {
        public GameObject PrefabGridView;
        public UnitView Team1Unit;
        public UnitView Team2Unit;
    }
}
