using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{

	[Header("Player Statics")]
	[SerializeField] float _moveSpeed;
	[SerializeField] float _sensitivity;
	[SerializeField] float _maxLookHeight;

	[Header("Player Elements")]
	[SerializeField] GameObject _camera;

	Rigidbody _rb;

	// Camera stuff
	float _pitch;
	float _yaw;

	// Inputs
	InputAction _moveAction;
	InputAction _lookAction;
	InputAction _hitAction;
	InputAction _shootAction;
	InputAction _pauseAction;

	void Start()
	{
		_moveAction = InputSystem.actions.FindAction("Move", true);
		_lookAction = InputSystem.actions.FindAction("Look", true);
		_hitAction = InputSystem.actions.FindAction("Melee", true);
		_shootAction = InputSystem.actions.FindAction("Shoot", true);
		_pauseAction = InputSystem.actions.FindAction("Pause", true);

		_rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		Look();
		Move();
	}

	// Movement stuff

	void Look()
	{
		Vector2 mousePos = _lookAction.ReadValue<Vector2>();
		_pitch += -mousePos.y * _sensitivity * Time.deltaTime;
		_yaw += mousePos.x * _sensitivity * Time.deltaTime;

		_pitch = Mathf.Clamp(_pitch, -_maxLookHeight, _maxLookHeight);

		_pitch %= 360f;
		_yaw %= 360f;

		Vector3 newCameraRot = _camera.transform.localEulerAngles;

		newCameraRot.x = _pitch;
		newCameraRot.y = _yaw;

		_camera.transform.localEulerAngles = newCameraRot;
	}

	void Move()
	{
		Vector2 inputPos = _moveAction.ReadValue<Vector2>();
		Vector3 newForce = Vector3.zero;
		newForce += _moveSpeed * inputPos.y * RightTransform();
		newForce += _moveSpeed * inputPos.x * ForwardTransform();

		_rb.AddForce(newForce);
	}

	Vector3 RightTransform()
	{
		float yawRadian = Mathf.PI * (-_yaw - 270f) / 180f;
		return new Vector3(Mathf.Cos(yawRadian), 0f, Mathf.Sin(yawRadian));
	}

	Vector3 ForwardTransform()
	{
		float yawRadian = Mathf.PI * -_yaw / 180f;
		return new Vector3(Mathf.Cos(yawRadian), 0f, Mathf.Sin(yawRadian));
	}

}