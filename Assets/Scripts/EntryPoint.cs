using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityView;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = new Vector2Int(20, 20);
    void Start()
    {
        var worldGen = new WorldGenerator(new UnityRandom());
        var model = worldGen.GenerateRandom(_size);

        var unityWorld = GetComponent<WorldView>();
        unityWorld.GenerateWorld(model);
        
        
    }
}

public sealed class UnityRandom : IRandom
{
    public int GetRandom(in int start, in int end)
    {
        return UnityEngine.Random.Range(start, end);
    }
}
