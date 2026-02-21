using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "WaveSO", menuName = "Scriptable Objects/WaveSO")]
public class WaveSO : ScriptableObject
{

    [Header("Wave Settings")]
    [SerializeField] List<Vector3> _spawnLocations;
    [SerializeField] List<GameObject> _spawnableEnemies;
    [SerializeField] int _numEnemies;
    [SerializeField] float _timeBetweenSpawns;

    public void SpawnWave()
    {

        List<Vector3> spawnLocations = new();
        List<GameObject> spawnEnemies = new();

        for (int i = 0; i < _numEnemies; i++)
        {
            spawnLocations.Add(_spawnLocations[Random.Range(0, _spawnLocations.Count)]);
            spawnEnemies.Add(_spawnableEnemies[Random.Range(0, _spawnableEnemies.Count)]);
        }

        WaveAuthority.Ref.PassSpawnEnemies(spawnLocations, spawnEnemies, _timeBetweenSpawns);
    }



}
