using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    [Header("Spawner Data")]
    [SerializeField] List<GameObject> _enemies;
    [SerializeField] float _timeBetweenSpawns;

    int _spawnIndex = 0;

    void OnEnable()
    {
        Invoke(nameof(SpawnEnemy), _timeBetweenSpawns);
    }

    void SpawnEnemy()
    {
        Instantiate(_enemies[_spawnIndex], transform.position, transform.rotation);
        _spawnIndex++;

        if (!(_spawnIndex >= _enemies.Count))
        {
            Invoke(nameof(SpawnEnemy), _timeBetweenSpawns);
        }
    }

}