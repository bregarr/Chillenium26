using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public static TutorialManager Ref { get; private set; }

    [Header("Elements")]
    [SerializeField] GameObject _tutorialEnemy;
    [SerializeField] GameObject _tutorialPrompt;

    bool _isInTutorial;

    void Start()
    {
        if (Ref)
        {
            Debug.LogWarning("Two tutorials in the scene");
        }
        Ref = this;
    }

    void Update()
    {
        if (!_tutorialEnemy)
        {
            _isInTutorial = false;
            if (_tutorialPrompt)
            {
                Destroy(_tutorialPrompt, 1.0f);
                _tutorialPrompt.GetComponentInChildren<TMP_Text>().text = "Good work!";
                WaveAuthority.Ref.StartFirstWave();
            }
        }
        else
        {
            _isInTutorial = true;
        }
    }

    public bool IsInTutorial()
    {
        return _isInTutorial;
    }

}
