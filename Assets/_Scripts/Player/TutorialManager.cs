using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

	public static TutorialManager Ref { get; private set; }

	[Header("Elements")]
	[SerializeField] GameObject _tutorialEnemy;
	[SerializeField] GameObject _tutorialPrompt;
	[SerializeField] Sprite[] _tutorialSprites;

	int _tutorialIndex;

	bool _startedSequence;
	bool _isInTutorial;

	void Start()
	{
		_startedSequence = false;
		_tutorialIndex = 0;
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
			if (_tutorialPrompt && !_startedSequence)
			{
				_startedSequence = true;
				Invoke(nameof(IncrementTutorials), 0.25f);
			}
		}
		else
		{
			_isInTutorial = true;
		}
	}

	void IncrementTutorials()
	{
		_tutorialPrompt.GetComponentInChildren<TMP_Text>().text = "";
		_tutorialPrompt.GetComponent<UnityEngine.UI.Image>().sprite = _tutorialSprites[_tutorialIndex];
		_tutorialIndex++;
		if (_tutorialIndex >= _tutorialSprites.Length)
		{
			Destroy(_tutorialPrompt, 3.5f);
			WaveAuthority.Ref.StartFirstWave();
		}
		else
		{
			Invoke(nameof(IncrementTutorials), 3.5f);
		}
	}

	public bool IsInTutorial()
	{
		return _isInTutorial;
	}

}
