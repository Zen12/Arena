using UnityEngine;

namespace Database
{
    [CreateAssetMenu(fileName = "PrefabDatabase", menuName = "_Config/PrefabDatabase", order = 1)]
    public sealed class PrefabDatabase : ScriptableObject
    {
        public GameObject PrefabGridView;
    }
}
