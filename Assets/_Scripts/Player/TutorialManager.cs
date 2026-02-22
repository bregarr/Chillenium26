using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public static TutorialManager Ref { get; private set; }

    [Header("Elements")]
    [SerializeField] GameObject _tutorialEnemy;

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
