using System.Collections.Generic;
using UnityEngine;

public class BossAuthority : MonoBehaviour
{

    public static BossAuthority Ref { get; private set; }

    [Header("Wave Data")]
    [SerializeField] List<WaveSO> _minionWaves = new();
    [SerializeField] float _waveFrequency;

    [Header("Boss Data")]
    [SerializeField] GameObject _boss;
    [SerializeField] Vector3 _bossSpawnLocation;

    float _lastWave;
    bool _startedBoss = false;

    void OnEnable()
    {
        if (Ref)
        {
            Debug.LogWarning("There are a lot of bosses huh");
        }
        Ref = this;
    }

    public void StartBossFight()
    {
        WaveAuthority.Ref.ActivateCutscene();
        _startedBoss = true;
    }

    public void SpawnBoss()
    {
        Instantiate(_boss, _bossSpawnLocation, transform.rotation);
        _lastWave = Time.time;
    }

    void SpawnWave()
    {
        if (_lastWave + _waveFrequency <= Time.time)
        {
            ForceSpawnWave();
        }
        Invoke(nameof(SpawnWave), Time.time - _lastWave - _waveFrequency);
    }

    public void ForceSpawnWave()
    {
        WaveSO thisWave = _minionWaves[Random.Range(0, _minionWaves.Count)];
        WaveAuthority.Ref.PassSpawnEnemies(thisWave.GetSpawns(), thisWave.GetEnemies(), thisWave.GetCooldown());
    }

    public bool GetStartedBoss() { return _startedBoss; }

}