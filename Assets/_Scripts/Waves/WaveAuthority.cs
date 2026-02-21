using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveAuthority : MonoBehaviour
{

	public static WaveAuthority Ref { get; private set; }
	public static PlayerControl PlayerRef { get; private set; }

	[Header("Wave Data")]
	[SerializeField] List<List<GameObject>> _waves;
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

		// Invoke(nameof(SpawnWave), _initialWaveDelay);
	}

	public static void SetPlayerRef(PlayerControl newRef)
	{
		PlayerRef = newRef;
	}

	void SpawnWave()
	{
		foreach (GameObject spawner in _waves[_waveIndex])
		{
			Instantiate(spawner);
		}

		_waveIndex++;

		if (_waveIndex < _waves.Count)
		{
			Invoke(nameof(SpawnWave), _timeBetweenWaves);
			return;
		}

		// The final wave was defeated

	}

}