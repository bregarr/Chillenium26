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
		_buttons.SetActive(true);

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
		StartCoroutine(nameof(LowerBackground));
	}

	public void TryRaiseBackground()
	{
		StartCoroutine(nameof(RaiseBackground));
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

	public void SetHold(float newY)
	{
		_backgroundHoldY = newY;
	}

	public bool GetIsActive() { return _isActive; }

}