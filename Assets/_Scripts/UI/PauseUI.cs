using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour
{

	public static PauseUI Ref { get; private set; }

	[Header("Elements")]
	[SerializeField] GameObject _pausedMenu;
	[SerializeField] SettingsMenu _settingsMenu;
	[SerializeField] TutorialMenu _controlsMenu;
	[SerializeField] GameObject _hudMenu;

	float _yRest;
	float _nextCanPause;
	float _pauseCooldown = .01f;

	bool _isPaused = false;

	InputAction _pauseAction;

	void OnEnable()
	{
		if (Ref)
		{
			Debug.LogWarning("Two pause menus in the scene!");
		}
		Ref = this;

		_yRest = Screen.height + 1f;
		_pauseAction = InputSystem.actions.FindAction("Pause", true);
		_nextCanPause = Time.time + _pauseCooldown;
		GetComponent<Canvas>().enabled = false;
		_controlsMenu.SetHold(_yRest);
	}

	void Update()
	{
		if (_pauseAction.WasPerformedThisFrame() && _nextCanPause <= Time.time)
		{
			_nextCanPause = Time.time + _pauseCooldown;
			switch (_isPaused)
			{
				case false:

					OpenPauseMenu();
					break;
				case true:

					ClosePauseMenu();
					break;
			}

		}
	}

	public void OpenPauseMenu()
	{
		_isPaused = true;
		_hudMenu.GetComponent<Canvas>().enabled = false;
		GetComponent<Canvas>().enabled = true;
		MoveToRest(_settingsMenu.gameObject);
		_controlsMenu.GoToHold();
		_pausedMenu.SetActive(true);
		Time.timeScale = 0f;
		CursorLock.Ref.UnlockCursor();
	}

	public void ClosePauseMenu()
	{
		_isPaused = false;
		_hudMenu.GetComponent<Canvas>().enabled = true;
		GetComponent<Canvas>().enabled = false;
		Time.timeScale = 1f;
		CursorLock.Ref.LockCursor();
		WaveAuthority.PlayerRef.UpdateInverts();
	}

	void MoveToRest(GameObject go)
	{
		Vector3 newPos = go.transform.position;
		newPos.y = _yRest;
		go.transform.position = newPos;
	}

	public void OpenSettings()
	{
		_settingsMenu.TryLowerBackground();
		if (_controlsMenu.GetIsActive())
		{
			_controlsMenu.TryRaiseBackground();
		}
	}

	public void CloseSettings()
	{
		_settingsMenu.TryRaiseBackground();
	}

	public void OpenControls()
	{
		_controlsMenu.TryLowerBackground();
		if (_settingsMenu.GetIsActive())
		{
			_settingsMenu.TryRaiseBackground();
		}
	}

	public void CloseControls()
	{
		_controlsMenu.TryRaiseBackground();
	}

	public bool GetIsPaused() { return _isPaused; }

}