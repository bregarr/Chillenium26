using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.Video;

public class WaveAuthority : MonoBehaviour
{

	public static WaveAuthority Ref { get; private set; }
	public static PlayerControl PlayerRef { get; private set; }

	[Header("Wave Data")]
	[SerializeField] List<WaveSO> _waves;
	[SerializeField] float _timeBetweenWaves;
	[SerializeField] float _initialWaveDelay;
	[SerializeField] GameObject _enemyHolder;

	[Header("Cutscene Data")]
	[SerializeField] Camera _cutsceneCamera;
	[SerializeField] VideoPlayer _vp;
	[SerializeField] VideoClip[] _videos;
	[SerializeField] Canvas _tvCanvas;

	bool _isInCutscene = false;
	int _waveIndex = 0;
	int _clickCount;
	GameObject _tv;

	void Start()
	{
		_waveIndex = 0;
		_isInCutscene = false;
		_cutsceneIndex = 0;
		if (Ref)
		{
			Debug.LogWarning("There are two wave authorities in the scene!");
		}
		Ref = this;

		_tv = _vp.gameObject;
		_tv.SetActive(true);
		ActivateCutscene();

		if (!_enemyHolder)
		{
			_enemyHolder = new GameObject();
		}
	}

	void FixedUpdate()
	{
		if (_isInCutscene && _vp.frame >= Convert.ToInt64(_vp.frameCount) - 1)
		{
			_isInCutscene = false;
			CutsceneEnd();
		}
		else if (_isInCutscene && _clickCount > 1)
		{
			CutsceneEnd();
		}
	}

	public void Click()
	{
		_clickCount++;
		_tvCanvas.enabled = true;
	}

	int _cutsceneIndex;

	public static void SetPlayerRef(PlayerControl newRef)
	{
		PlayerRef = newRef;
	}

	void SpawnWave()
	{
		Debug.Log(_waveIndex);
		if (_waves.Count > 0 && _waveIndex < _waves.Count)
		{
			_waves[_waveIndex].SpawnWave();
		}

		_waveIndex++;

		if (_waveIndex < _waves.Count)
		{
			Invoke(nameof(SpawnWave), _timeBetweenWaves);
			return;
		}
		if (_waveIndex == _waves.Count - 1)
		{

		}

		if (_enemyHolder.GetComponentsInChildren<Transform>().Count() < 2 && !BossAuthority.Ref.GetStartedBoss())
		{
			BossAuthority.Ref.StartBossFight();
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
			GameObject newEnemy = Instantiate(enemies[i], locations[i], transform.rotation, _enemyHolder.transform);
			newEnemy.GetComponent<Enemy>().UpdateDrop(DiceAuthority.Ref.GetRandomDiceType());
			yield return new WaitForSeconds(cooldown);
		}
	}

	public void ActivateCutscene()
	{
		_tvCanvas.enabled = false;
		_tv.SetActive(true);
		_clickCount = 0;
		_isInCutscene = true;
		_vp.clip = _videos[_cutsceneIndex];
		PlayerRef.GetCamera().enabled = false;
		_cutsceneCamera.enabled = true;
		AudioManager.MusicRef.stopMusic();
		PauseUI.Ref.ClosePauseMenu();
		HudUI.Ref.HideCanvas();
		_vp.Play();


	}

	void CutsceneEnd()
	{
		_tvCanvas.enabled = false;
		_cutsceneIndex++;
		_cutsceneIndex %= _videos.Length;
		AudioManager.MusicRef.startMusic();
		HudUI.Ref.ShowCanvas();
		_isInCutscene = false;

		_cutsceneCamera.enabled = false;
		PlayerRef.GetCamera().enabled = true;
		_tv.SetActive(false);
		if (BossAuthority.Ref && BossAuthority.Ref.GetStartedBoss())
		{
			BossAuthority.Ref.SpawnBoss();
		}
	}

	public void StartFirstWave()
	{
		if (_waveIndex == 0)
		{
			Debug.Log("Spawning first wave!");
			SpawnWave();
		}
	}

	public bool IsInCutscene() { return _isInCutscene; }

}