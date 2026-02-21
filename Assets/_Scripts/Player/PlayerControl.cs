using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum eWeapon
{
	Sword, Projectile
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerControl : Character
{

	[Header("Player Statics")]
	[SerializeField] float _sensitivity;
	[SerializeField] float _maxSpeed;
	[SerializeField] float _maxLookHeight;
	[SerializeField] float _meleeCooldown;
	[SerializeField] float _projectileCooldown;
	[SerializeField] int _maxDice;
	[SerializeField] float _runThreshold;
	[SerializeField] float _projectileSpeed;
	[SerializeField] float _projectileTTL;

	[Header("Buffable Statics")]
	[SerializeField] float _moveSpeed;
	[SerializeField] float _moveSpeedScaling;
	[SerializeField] float _meleeDamage;
	[SerializeField] float _meleeDamageScaling;
	[SerializeField] float _projectileDamage;
	[SerializeField] float _projectileDamageScaling;
	[SerializeField] float _defense;
	[SerializeField] float _defenseScaling;
	[SerializeField] float _healthRegen;
	[SerializeField] float _healthRegenScaling;

	[Header("Player Elements")]
	[SerializeField] GameObject _camera;
	[SerializeField] GameObject _sword;
	[SerializeField] GameObject _hand;

	Queue<Dice> _dice;
	Queue<int> _ammo;

	Queue<Dice> _buffs; // Used by HandleBuffs

	eWeapon _activeWeapon = eWeapon.Sword;
	float _lastAttackTime;
	Health _health;

	Rigidbody _rb;
	PlayerAnimator _anim;

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

		_dice = new Queue<Dice>();
		_ammo = new Queue<int>();
		_buffs = new Queue<Dice>();

		_rb = GetComponent<Rigidbody>();
		_anim = GetComponent<PlayerAnimator>();
		_health = GetComponent<Health>();
		WaveAuthority.SetPlayerRef(this);

		_lastAttackTime = Time.time;

		if (!PlayerPrefs.HasKey("sensitivity"))
		{
			PlayerPrefs.SetFloat("sensitivity", 55f);
		}

		_sensitivity = PlayerPrefs.GetFloat("sensitivity");
	}

	void Update()
	{
		Look();
		Move();

		if (_hitAction.WasCompletedThisFrame())
		{
			Melee();
		}
		else if (_shootAction.WasCompletedThisFrame())
		{
			Throw();
		}
	}

	// Handle Current Buffs
	void AddBuff(Dice dice)
	{
		switch (dice.GetBuffType())
		{
			case eBuffType.Health:
				_healthRegen += dice.sideNum * _healthRegenScaling;
				break;
			case eBuffType.Damage:
				_meleeDamage *= 1 + dice.sideNum * _meleeDamageScaling;
				break;
			case eBuffType.Speed:
				_moveSpeed *= 1 + dice.sideNum * _moveSpeedScaling;
				break;
			case eBuffType.Defense:
				_defense *= 1 + dice.sideNum * _defenseScaling;
				break;
			case eBuffType.Ammo:
				int[] diceTypes = { 4, 6, 8, 12, 20 };
				for (int i = 0; i < dice.sideNum; i++)
				{
					_ammo.Enqueue(diceTypes[(int)Random.Range(0f, diceTypes.Length)]);
				}
				break;
		}
	}
	void RemoveBuff(Dice dice)
	{
		switch (dice.GetBuffType())
		{
			case eBuffType.Health:
				_healthRegen -= dice.sideNum * _healthRegenScaling;
				break;
			case eBuffType.Damage:
				_meleeDamage /= 1 + dice.sideNum * _meleeDamageScaling;
				break;
			case eBuffType.Speed:
				_moveSpeed /= 1 + dice.sideNum * _moveSpeedScaling;
				break;
			case eBuffType.Defense:
				_defense /= 1 + dice.sideNum * _defenseScaling;
				break;
			case eBuffType.Ammo:
				// Do nothing
				break;
		}
	}
	void HandleBuffs(Dice newDice, Dice oldDice)
	{
		if (oldDice != null)
		{
			RemoveBuff(oldDice);
		}
		AddBuff(newDice);
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

		// Clamp the velocity
		Vector3 currVel = _rb.linearVelocity;
		currVel.x = Mathf.Clamp(currVel.x, -_maxSpeed, _maxSpeed);
		currVel.z = Mathf.Clamp(currVel.z, -_maxSpeed, _maxSpeed);
		_rb.linearVelocity = currVel;

		if (Mathf.Abs(currVel.magnitude) >= _runThreshold)
		{
			_anim.UpdateWalkState(eWalkState.Running);
		}
		else if (Mathf.Abs(currVel.magnitude) > 0.0001f)
		{
			_anim.UpdateWalkState(eWalkState.Walking);
		}
		else
		{
			_anim.UpdateWalkState(eWalkState.Idle);
		}
	}

	public Vector3 RightTransform()
	{
		float yawRadian = Mathf.PI * (-_yaw - 270f) / 180f;
		return new Vector3(Mathf.Cos(yawRadian), 0f, Mathf.Sin(yawRadian));
	}

	public Vector3 ForwardTransform()
	{
		float yawRadian = Mathf.PI * -_yaw / 180f;
		return new Vector3(Mathf.Cos(yawRadian), 0f, Mathf.Sin(yawRadian));
	}

	public override void DeathEvent()
	{
		// Kill this guy
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void Melee()
	{
		if (_lastAttackTime + _meleeCooldown > Time.time)
		{
			// Melee is still on cooldown
			return;
		}

		if (_activeWeapon != eWeapon.Sword)
		{
			// Set the sword to active
			_activeWeapon = eWeapon.Sword;
		}

		// Attack
		_anim.Slash();

		_lastAttackTime = Time.time;
	}

	void Throw()
	{
		if (_lastAttackTime + _projectileCooldown > Time.time)
		{
			// Projectile is still on cooldown
			return;
		}

		if (_activeWeapon != eWeapon.Projectile)
		{
			// Set the projectile to active
			_activeWeapon = eWeapon.Projectile;
		}

		// If there isnt available ammo, don't shoot
		// if (GetAmmo() <= 0)
		// {
		// 	return;
		// }

		// Attack
		_hand.GetComponent<Hand>().Throw();
		_anim.Throw();
		_lastAttackTime = Time.time;
	}

	public void AddDice(Dice dice)
	{
		_dice.Enqueue(dice);
		Dice oldDice = null;
		if (!(_dice.Count >= _maxDice))
		{
			oldDice = (Dice)_dice.Dequeue();

			// Add oldDice to Dice Bag
			AddAmmo(oldDice);
		}
		HandleBuffs(dice, oldDice);

		_anim.ChangeDie();
	}

	public void AddAmmo(Dice dice)
	{
		int oldDice = (int)dice.GetBuffType();
		_ammo.Enqueue(oldDice);
	}

	public int GetAmmo()
	{
		// Make sure we have ammo
		if (_ammo.Count == 0)
		{
			// For testing
			_ammo.Enqueue(6);
			//return 0;
		}

		return _ammo.Dequeue();
	}
  public int GetAmmoCount()
	{
		return _ammo.Count;
	}

	public float GetMeleeDamage()
	{
		return _meleeDamage;
	}

	public void SetMeleeDamage(float damage)
	{
		_meleeDamage = damage;
	}

	public float GetProjectileDamage()
	{
		return _projectileDamage;
	}

	public void SetProjectileDamage(float damage)
	{
		_projectileDamage = damage;
	}

	public float GetMoveSpeed()
	{
		return _moveSpeed;
	}

	public void SetMoveSpeed(float speed)
	{
		_moveSpeed = speed;
	}

	public float GetDefense()
	{
		return _defense;
	}

	public void SetDefense(float defense)
	{
		_defense = defense;
	}
	public float GetHealthRegen()
	{
		return _healthRegen;
	}

	public void SetHealthRegen(float healthRegen)
	{
		_healthRegen = healthRegen;
	}

	public int GetMaxDice() { return _maxDice; }

	public float GetProjectileSpeed() { return _projectileSpeed; }

	public float GetProjectileTTL() { return _projectileTTL; }

	public float GetPitch() { return _pitch; }

}