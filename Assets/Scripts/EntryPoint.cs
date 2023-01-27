using System.Collections;
using System.Collections.Generic;
using Permutation;
using Simulation;
using UnityEngine;
using UnityView;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = new Vector2Int(20, 20);

    private UnityTime _time;
    private BattleSimulation _simulation;
    
    IEnumerator Start()
    {
        var random = new UnityRandom();
        var worldGen = new WorldGenerator(random);
        var model = worldGen.GenerateRandom(_size);
        var unityWorld = GetComponent<WorldView>();
        unityWorld.GenerateWorld(model);
        
        _time = new UnityTime();

        var permutation = new PermutationController(_time);
        permutation.Add(unityWorld);
        
        _simulation = new BattleSimulation(_time, permutation, model, random);

        while (true)
        {
            yield return new WaitForSeconds(1f);
            _time.CurrentTime += 1f;
        }
    }

    public void Update()
    {
        //var result = _simulation.NextMoves();
    }
}

public sealed class UnityTime : ITimeline
{
    public float CurrentTime { get; internal set; }
}

public sealed class UnityRandom : IRandom
{
    public int GetRandom(in int start, in int end)
    {
        return UnityEngine.Random.Range(start, end);
    }
}
