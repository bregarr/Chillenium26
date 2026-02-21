using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAuthority : MonoBehaviour
{

	public static WaveAuthority Ref { get; private set; }
	public static PlayerControl PlayerRef { get; private set; }

	[Header("Wave Data")]
	[SerializeField] List<WaveSO> _waves;
	[SerializeField] float _timeBetweenWaves;
	[SerializeField] float _initialWaveDelay;

	int _waveIndex = 0;

	void Start()
	{
		if (Ref)
		{
			Debug.LogWarning("There are two wave authorities in the scene!");
		}
		Ref = this;

		Invoke(nameof(SpawnWave), _initialWaveDelay);
	}

	public static void SetPlayerRef(PlayerControl newRef)
	{
		PlayerRef = newRef;
	}

	void SpawnWave()
	{
		_waves[_waveIndex].SpawnWave();

		_waveIndex++;

		if (_waveIndex < _waves.Count)
		{
			Invoke(nameof(SpawnWave), _timeBetweenWaves);
			return;
		}

		// The final wave was defeated

	}

	public void PassSpawnEnemies(List<Vector3> locations, List<GameObject> enemies, float cooldown)
	{
		StartCoroutine(SpawnEnemies(locations, enemies, cooldown));
	}

	IEnumerator SpawnEnemies(List<Vector3> locations, List<GameObject> enemies, float cooldown)
	{
		for (int i = 0; i < locations.Count; i++)
		{
			Instantiate(enemies[i], locations[i], transform.rotation);
			yield return new WaitForSeconds(cooldown);
		}
	}
}