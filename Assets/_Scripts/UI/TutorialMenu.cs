using UnityEngine;
using System.Collections;

public class TutorialMenu : MonoBehaviour
{

	[Header("Elements")]
	[SerializeField] GameObject _background;

	[Header("Element Data")]
	[SerializeField] float _backgroundStepTime;
	[SerializeField] float _backgroundStepSize;


	bool _isActive = false;

	bool _isLowering = false;
	bool _isRaising = false;

	float _backgroundHoldY;

	public void TryRaiseBackground()
	{
		_isRaising = true;
		_isLowering = false;
		StartCoroutine(nameof(RaiseBackground));
	}

	public void TryLowerBackground()
	{
		_isRaising = false;
		_isLowering = true;
		_background.SetActive(true);
		StartCoroutine(nameof(LowerBackground));
	}

	IEnumerator LowerBackground()
	{
		_isActive = true;
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

	public void SetHold(float newY)
	{
		_backgroundHoldY = newY;
		GoToHold();
	}

	public void GoToHold()
	{
		Vector3 newPos = transform.position;
		newPos.y = _backgroundHoldY * 1.5f;
		transform.position = newPos;
	}

	public bool GetIsActive() { return _isActive; }

}