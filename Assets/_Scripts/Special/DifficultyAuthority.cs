using UnityEngine;

public enum eDifficulty
{
    Hard = 0, Extreme = 1, Impossible = 2
}

public class DifficultyAuthority : MonoBehaviour
{

    public static DifficultyAuthority Ref { get; private set; }

    eDifficulty _difficulty;

    [Header("Difficulty Stats (Edit in vsc)")]
    [SerializeField] float[] _healthBuff = { 2f, 1.5f, 1f };
    [SerializeField] float[] _damageBuff = { 1.8f, 1.4f, 1f };
    [SerializeField] float[] _defenseBuff = { 1.5f, 1.2f, 1.0f };
    [SerializeField] float[] _attackSpeedBuff = { 3.0f, 1.8f, 1.0f };
    [SerializeField] float[] _enemySpawnRates = { 2.0f, 1.5f, 1.0f };

    public eDifficulty GetDifficulty() { return _difficulty; }

    public float GetHealthBuff() { return _healthBuff[(int)_difficulty]; }
    public float GetDamageBuff() { return _damageBuff[(int)_difficulty]; }
    public float GetDefenseBuff() { return _defenseBuff[(int)_difficulty]; }
    public float GetSpeedBuff() { return _attackSpeedBuff[(int)_difficulty]; }
    public float GetSpawnRateBuff() { return _enemySpawnRates[(int)_difficulty]; }

    public void UpdateDifficulty(eDifficulty difficulty)
    {
        _difficulty = difficulty;
        PlayerPrefs.SetInt("difficulty", (int)difficulty);
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("difficulty"))
        {
            PlayerPrefs.SetInt("difficulty", 0);
        }
        _difficulty = (eDifficulty)PlayerPrefs.GetInt("difficulty");

        Ref = this;
    }

}