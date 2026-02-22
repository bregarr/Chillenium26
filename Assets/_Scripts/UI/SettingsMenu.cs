using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

	[Header("Elements")]
	[SerializeField] GameObject _opacity;
	[SerializeField] GameObject _buttons;
	[SerializeField] GameObject _background;

	[Header("Setting Elements")]
	[SerializeField] Toggle _xInvertToggle;
	[SerializeField] Toggle _yInvertToggle;
	[SerializeField] Slider _sensitivitySlider;
	[SerializeField] TMP_Text _sensitivityLabel;
	[SerializeField] Slider _musicSlider;
	[SerializeField] TMP_Text _musicLabel;
	[SerializeField] Slider _sfxSlider;
	[SerializeField] TMP_Text _sfxLabel;

	[Header("Settings")]
	[SerializeField] float _targetOpacity;
	[SerializeField] float _opacityStepTime;
	[SerializeField] float _backgroundStepTime;
	[SerializeField] float _backgroundStepSize;

	[Header("Setting Settings")]
	[SerializeField] float _maxSensitivity = 120f;
	[SerializeField] float _minSensitivity = 10f;

	bool _isActive = false;

	bool _isIncrementing = false;
	bool _isDecrementing = false;
	bool _isLowering = false;
	bool _isRaising = false;
	bool _useButtons = false;

	float _backgroundHoldY;

	void Start()
	{
		_backgroundHoldY = Screen.height + 1f;
		HideMenu();

		// Default Sensitivity Value
		if (!PlayerPrefs.HasKey("sensitivity"))
		{
			PlayerPrefs.SetFloat("sensitivity", 55f);
		}
		float sensitivity = PlayerPrefs.GetFloat("sensitivity");
		_sensitivityLabel.text = sensitivity.ToString();
		_sensitivitySlider.value = (sensitivity - _minSensitivity) / _maxSensitivity;

		// Default Music Value
		if (!PlayerPrefs.HasKey("musicVolume"))
		{
			PlayerPrefs.SetFloat("musicVolume", 100f);
		}
		float musicVolume = PlayerPrefs.GetFloat("musicVolume");
		_musicLabel.text = musicVolume.ToString("00%");
		_musicSlider.value = musicVolume / 200f;

		// Default SFX Value
		if (!PlayerPrefs.HasKey("sfxVolume"))
		{
			PlayerPrefs.SetFloat("sfxVolume", 100f);
		}
		float sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
		_sfxLabel.text = sensitivity.ToString("00%");
		_sfxSlider.value = sfxVolume / 200f;
	}

	public void EnableMenu()
	{

		_isIncrementing = true;
		_isDecrementing = false;
		_isLowering = true;
		_isRaising = false;

		_opacity.SetActive(true);
		_background.SetActive(true);

		IncrementOpacity();
		TryLowerBackground();
		_useButtons = true;

	}

	public void DisableMenu()
	{
		if (!_isActive)
		{
			return;
		}

		_isDecrementing = true;
		_isIncrementing = false;
		_isLowering = false;
		_isRaising = true;
		DecrementOpacity();
		TryRaiseBackground();
		_buttons.SetActive(false);

	}

	public void HideMenu()
	{

		_isIncrementing = false;
		_isDecrementing = false;

		// Reset opacity to 0
		Color currColor = _opacity.GetComponent<UnityEngine.UI.Image>().color;
		currColor.a = 0f;
		_opacity.GetComponent<UnityEngine.UI.Image>().color = currColor;
		_opacity.SetActive(false);

		// Hide the buttons
		_buttons.SetActive(false);

		// Move the background
		Vector3 currPos = _background.transform.position;
		currPos.y = _backgroundHoldY;
		_background.transform.position = currPos;
		_background.SetActive(false);

	}

	void IncrementOpacity()
	{
		Color currColor = _opacity.GetComponent<UnityEngine.UI.Image>().color;
		if (currColor.a < _targetOpacity && !_isDecrementing)
		{
			currColor.a += .1f;
			_opacity.GetComponent<UnityEngine.UI.Image>().color = currColor;
			Invoke(nameof(IncrementOpacity), _opacityStepTime);
			return;
		}
		_isIncrementing = false;
	}

	void DecrementOpacity()
	{
		Color currColor = _opacity.GetComponent<UnityEngine.UI.Image>().color;
		if (currColor.a > 0f && !_isIncrementing)
		{
			currColor.a -= .1f;
			_opacity.GetComponent<UnityEngine.UI.Image>().color = currColor;
			Invoke(nameof(DecrementOpacity), _opacityStepTime);
			return;
		}
		_isDecrementing = false;
		_opacity.SetActive(false);
	}

	public void TryLowerBackground()
	{
		if (_isActive)
		{
			_isLowering = false;
			_isRaising = true;
			StartCoroutine(nameof(RaiseBackground));
		}
		else
		{
			_isLowering = true;
			_isRaising = false;
			StartCoroutine(nameof(LowerBackground));
		}
	}

	public void TryRaiseBackground()
	{
		if (!_isActive)
		{
			_isLowering = true;
			_isRaising = false;
			StartCoroutine(nameof(LowerBackground));
		}
		else
		{
			_isLowering = false;
			_isRaising = true;
			StartCoroutine(nameof(RaiseBackground));
		}
	}

	IEnumerator LowerBackground()
	{
		_isActive = true;
		_background.SetActive(true);
		Vector3 currPos = _background.transform.position;

		while (currPos.y > _backgroundHoldY / 2f && !_isRaising)
		{
			currPos.y -= _backgroundStepSize;
			_background.transform.position = currPos;
			yield return new WaitForSecondsRealtime(_backgroundStepTime);
		}
		_isLowering = false;
		if (_useButtons)
		{
			_buttons.SetActive(true);
		}
	}

	IEnumerator RaiseBackground()
	{
		Vector3 currPos = _background.transform.position;

		while (currPos.y < _backgroundHoldY * 1.5f && !_isLowering)
		{
			currPos.y += _backgroundStepSize;
			_background.transform.position = currPos;
			yield return new WaitForSecondsRealtime(_backgroundStepTime);
		}
		_isRaising = false;
		_background.SetActive(false);
		_isActive = false;
	}

	public void SetInvertX()
	{
		if (_xInvertToggle.isOn)
		{
			PlayerPrefs.SetInt("xInvert", -1);
		}
		else
		{
			PlayerPrefs.SetInt("xInvert", 1);
		}
	}

	public void SetInvertY()
	{
		if (_yInvertToggle.isOn)
		{
			PlayerPrefs.SetInt("yInvert", -1);
		}
		else
		{
			PlayerPrefs.SetInt("yInvert", 1);
		}
	}

	public void SensitivitySliderChange()
	{
		float newSens = _sensitivitySlider.value * _maxSensitivity + _minSensitivity;
		_sensitivityLabel.text = newSens.ToString("00");
		PlayerPrefs.SetFloat("sensitivity", newSens);
	}

	public void MusicSliderChange()
	{
		float newVol = _musicSlider.value * 200;
		_musicLabel.text = newVol.ToString("00");
		PlayerPrefs.SetFloat("musicVolume", newVol);
		if (AudioManager.Ref)
		{
			AudioManager.Ref.UpdateVolume();
		}
	}

	public void SfxSliderChange()
	{
		float newVol = _sfxSlider.value * 200;
		_sfxLabel.text = newVol.ToString("00");
		PlayerPrefs.SetFloat("sfxVolume", newVol);
		if (AudioManager.Ref)
		{
			AudioManager.Ref.UpdateVolume();
		}
	}

	public void SetHold(float newY)
	{
		_backgroundHoldY = newY;
	}

	public bool GetIsActive() { return _isActive; }

}