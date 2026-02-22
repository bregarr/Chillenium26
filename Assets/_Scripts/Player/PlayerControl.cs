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

	float _diffHealthBuff;
	float _diffDamageBuff;
	float _diffDefenseBuff;
	float _diffSpeedBuff;

	[Header("Player Statics")]
	[SerializeField] float _sensitivity;
	[SerializeField] float _maxSpeed;
	[SerializeField] float _drag;
	[SerializeField] float _maxLookHeight;
	[SerializeField] float _meleeCooldown;
	[SerializeField] float _projectileCooldown;
	[SerializeField] int _maxDice;
	[SerializeField] float _runThreshold;
	[SerializeField] float _projectileSpeed;
	[SerializeField] float _projectileTTL;
	[SerializeField] bool _isInArena = false;

	[Header("References")]
	[SerializeField] UIBuffs _uiBuff;

	[Header("Buffable Statics")]
	[SerializeField] float _baseMoveSpeed;
	[SerializeField] float _moveSpeed;
	[SerializeField] float _moveSpeedScaling;
	[SerializeField] float _baseMeleeDamage;
	[SerializeField] float _meleeDamage;
	[SerializeField] float _meleeDamageScaling;
	[SerializeField] float _baseProjectileDamage;
	[SerializeField] float _projectileDamage;
	[SerializeField] float _projectileDamageScaling;
	[SerializeField] float _baseDefense;
	[SerializeField] float _defense;
	[SerializeField] float _defenseScaling;
	[SerializeField] float _baseHealthRegen;
	[SerializeField] float _healthRegen;
	[SerializeField] float _healthRegenScaling;

	[Header("Player Elements")]
	[SerializeField] GameObject _camera;
	[SerializeField] GameObject _sword;
	[SerializeField] GameObject _hand;
	[SerializeField] Camera _deadCamera;
	[SerializeField] GameObject _winDecal;

	Queue<Dice> _dice;
	Queue<int> _ammo;

	float _invertX = 1f;
	float _invertY = 1f;

	Queue<Dice> _buffs; // Used by HandleBuffs

	eWeapon _activeWeapon = eWeapon.Sword;
	float _lastAttackTime;
	Health _health;

	Rigidbody _rb;
	PlayerAnimator _anim;

	float _healthRegenTimer;

	// Camera stuff
	float _pitch;
	float _yaw;

	// Cheats
	bool _diceCheat;
	float _throwCheat;

	bool _isDead = false;

	// Inputs
	InputAction _moveAction;
	InputAction _lookAction;
	InputAction _hitAction;
	InputAction _shootAction;

	void Start()
	{
		_isDead = false;
		_moveAction = InputSystem.actions.FindAction("Move", true);
		_lookAction = InputSystem.actions.FindAction("Look", true);
		_hitAction = InputSystem.actions.FindAction("Melee", true);
		_shootAction = InputSystem.actions.FindAction("Shoot", true);

		DifficultyAuthority auth = DifficultyAuthority.Ref;
		_diffHealthBuff = auth.GetHealthBuff();
		_diffDamageBuff = auth.GetDamageBuff();
		_diffDefenseBuff = auth.GetDefenseBuff();
		_diffSpeedBuff = auth.GetSpeedBuff();

		_dice = new Queue<Dice>();
		_ammo = new Queue<int>();
		// _buffs = new Queue<Dice>();

		_rb = GetComponent<Rigidbody>();
		_anim = GetComponent<PlayerAnimator>();
		_health = GetComponent<Health>();
		WaveAuthority.SetPlayerRef(this);

		SetMaxHealth(_health.GetMaxHealth() * _diffHealthBuff);

		_lastAttackTime = Time.time;

		if (!PlayerPrefs.HasKey("sensitivity"))
		{
			PlayerPrefs.SetFloat("sensitivity", 55f);
		}

		_sensitivity = PlayerPrefs.GetFloat("sensitivity");

		if (!PlayerPrefs.HasKey("xInvert") || Mathf.Abs(PlayerPrefs.GetFloat("xInvert")) != 1f)
		{
			PlayerPrefs.SetFloat("xInvert", 1f);
		}
		_invertX = PlayerPrefs.GetFloat("xInvert");
		if (!PlayerPrefs.HasKey("yInvert") || Mathf.Abs(PlayerPrefs.GetFloat("yInvert")) != 1f)
		{
			PlayerPrefs.SetFloat("yInvert", 1f);
		}
		_invertY = PlayerPrefs.GetFloat("yInvert");

		if (_isInArena)
		{
			_camera.transform.localEulerAngles = new Vector3(-11.5f, -88.5f, 0f);
			_yaw = 272.93f;
			_pitch = -7f;
		}

		if (PlayerPrefs.HasKey("diceCheat"))
		{
			_diceCheat = PlayerPrefs.GetInt("diceCheat") == 1;
		}
		else
		{
			_diceCheat = false;
		}

		if (PlayerPrefs.HasKey("speedCheat"))
		{
			_throwCheat = PlayerPrefs.GetFloat("speedCheat");
		}
		else
		{
			_throwCheat = 1f;
		}

		if (_throwCheat < 1f)
		{
			PlayerPrefs.SetFloat("speedCheat", 1f);
			_throwCheat = 1f;
		}
	}

	void Update()
	{
		if (_throwCheat != 1.0f && _throwCheat != 10.0f)
		{
			PlayerPrefs.SetFloat("speedCheat", 1f);
			_throwCheat = 1.0f;
		}

		bool isInCutscene = WaveAuthority.Ref.IsInCutscene();
		if (!PauseUI.Ref.GetIsPaused() && !isInCutscene)
		{
			Look();
		}

		if ((!TutorialManager.Ref || !TutorialManager.Ref.IsInTutorial()) && !isInCutscene)
		{
			Move();
		}

		if (_hitAction.WasCompletedThisFrame())
		{
			if (isInCutscene)
			{
				WaveAuthority.Ref.Click();
			}
			else
			{
				Melee();
			}
		}
		else if (_shootAction.WasCompletedThisFrame() && !isInCutscene)
		{
			Throw();
		}

		// Slow down faster
		Vector2 inputPos = _moveAction.ReadValue<Vector2>();
		if (inputPos.magnitude < 0.1)
		{
			_rb.linearVelocity /= 4;
		}

		// Health Regen
		if (_healthRegen > 0)
		{
			if (_healthRegenTimer < 0)
			{
				_healthRegenTimer = 1f;
				if (_health.GetHealth() + _healthRegen > _health.GetMaxHealth())
				{
					_health.AddHealth(_health.GetMaxHealth() - _health.GetHealth());
				}
				else
				{
					_health.AddHealth(_healthRegen);
				}
			}
			_healthRegenTimer -= Time.deltaTime;
		}
	}

	// Handle Current Buffs
	void AddBuff(Dice dice)
	{
		AudioManager.Ref.playSFX("PowerUP", 0.75f);
		switch (dice.GetBuffType())
		{
			case eBuffType.Health:
				_healthRegen += dice.sideNum * _healthRegenScaling;
				AudioManager.MusicRef.healthMusic(true);
				break;
			case eBuffType.Damage:
				_meleeDamage *= 1 + dice.sideNum * _meleeDamageScaling;
				_projectileDamage *= 1 + dice.sideNum * _projectileDamageScaling;
				AudioManager.MusicRef.damageMusic(true);
				break;
			case eBuffType.Speed:
				_maxSpeed *= 1 + dice.sideNum * _moveSpeedScaling;
				AudioManager.MusicRef.speedMusic(true);
				break;
			case eBuffType.Defense:
				_defense *= 1 + dice.sideNum * _defenseScaling;
				AudioManager.MusicRef.defenseMusic(true);
				break;
			case eBuffType.Ammo:
				int[] diceTypes = { 4, 6, 8, 12, 20 };
				for (int i = 0; i < dice.sideNum; i++)
				{
					_ammo.Enqueue(diceTypes[(int)Random.Range(0f, diceTypes.Length)]);
				}
				AudioManager.MusicRef.ammoMusic(true);
				break;
		}
	}
	void RemoveBuff(Dice dice)
	{
		switch (dice.GetBuffType())
		{
			case eBuffType.Health:
				_healthRegen -= dice.sideNum * _healthRegenScaling;
				if (Mathf.Abs(_healthRegen - _baseHealthRegen) < 0.05)
				{
					_healthRegen = _baseHealthRegen;
					AudioManager.MusicRef.healthMusic(false);
				}
				break;
			case eBuffType.Damage:
				_meleeDamage /= 1 + dice.sideNum * _meleeDamageScaling;
				if (Mathf.Abs(_meleeDamage - _baseMeleeDamage) < 0.05)
				{
					_meleeDamage = _baseMeleeDamage;
					AudioManager.MusicRef.damageMusic(false);
				}
				_projectileDamage /= 1 + dice.sideNum * _projectileDamageScaling;
				if (Mathf.Abs(_projectileDamage - _baseProjectileDamage) < 0.05)
				{
					_projectileDamage = _baseProjectileDamage;
				}
				break;
			case eBuffType.Speed:
				_maxSpeed /= 1 + dice.sideNum * _moveSpeedScaling;
				if (Mathf.Abs(_maxSpeed - _baseMoveSpeed) < 0.05)
				{
					_maxSpeed = _baseMoveSpeed;
				}
				AudioManager.MusicRef.speedMusic(false);
				break;
			case eBuffType.Defense:
				_defense /= 1 + dice.sideNum * _defenseScaling;
				if (Mathf.Abs(_defense - _baseDefense) < 0.05)
				{
					_defense = _baseDefense;
					AudioManager.MusicRef.defenseMusic(false);
				}
				break;
			case eBuffType.Ammo:
				// Do nothing
				AudioManager.MusicRef.ammoMusic(false);
				break;
		}
	}

	// Movement stuff

	void Look()
	{
		if (Cursor.lockState != CursorLockMode.Locked)
		{
			return;
		}
		Vector2 mousePos = _lookAction.ReadValue<Vector2>();
		_pitch += _invertY * -mousePos.y * _sensitivity * Time.deltaTime;
		_yaw += _invertX * mousePos.x * _sensitivity * Time.deltaTime;

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
		Vector3 currVel = _rb.linearVelocity;

		if (!(currVel.magnitude >= _maxSpeed))
		{
			_rb.AddForce(newForce);
		}

		// Clamp the velocity
		if (currVel.y > 0f)
		{
			currVel.y = 0f;
		}

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
		float yawRadian = Mathf.PI * (-_yaw - 270.0f) / 180.0f;
		return new Vector3(Mathf.Cos(yawRadian), 0.0f, Mathf.Sin(yawRadian));
	}

	public Vector3 ForwardTransform()
	{
		float yawRadian = Mathf.PI * -_yaw / 180.0f;
		return new Vector3(Mathf.Cos(yawRadian), 0.0f, Mathf.Sin(yawRadian));
	}

	public override void DeathEvent()
	{
		if (_isDead)
		{
			return;
		}
		_isDead = true;
		// Kill this guy
		_deadCamera.enabled = true;
		HudUI.Ref.HideCanvas();
		_camera.GetComponent<Camera>().enabled = false;
		AudioManager.MusicRef.stopMusic();
		AudioManager.Ref.playSFX("guppy_Death");
		Invoke(nameof(GoToMenu), 3.5f);
	}

	void GoToMenu()
	{
		SceneManager.LoadScene("Scenes/MainMenu");
	}

	void Melee()
	{
		if (_lastAttackTime + _meleeCooldown / _diffSpeedBuff / _throwCheat > Time.time)
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
		AudioManager.Ref.playSFX("slash");
		_anim.Slash();

		_lastAttackTime = Time.time;
	}

	void Throw()
	{
		if (_lastAttackTime + _projectileCooldown / _diffSpeedBuff / _throwCheat > Time.time)
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
		if (GetAmmoCount() <= 0 && !_diceCheat)
		{
			return;
		}
		else
		{

			// Attack
			_hand.GetComponent<Hand>().Throw();
			_anim.Throw();
			_lastAttackTime = Time.time;
		}

	}

	public void AddDice(Dice dice)
	{
		_dice.Enqueue(dice);
		Dice oldDice = null;
		if (_dice.Count > _maxDice)
		{
			oldDice = (Dice)_dice.Dequeue();
			RemoveBuff(oldDice);
			// Add oldDice to Dice Bag
			AddAmmo(oldDice);
		}
		AddBuff(dice);
		_uiBuff.AddBuff(dice);

		_anim.ChangeDie();
	}

	public void TakeDamage(float amount)
	{
		_health.TakeDamage(amount);
	}

	public void AddAmmo(Dice dice)
	{
		int oldDice = (int)dice.GetBuffType();
		_ammo.Enqueue(oldDice);
	}

	public int GetAmmo()
	{
		// Make sure we have ammo
		if (_ammo.Count == 0 && _diceCheat)
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

	public void SetMaxHealth(float amount)
	{
		_health.SetMaxHealth(amount);
	}

	public float GetMeleeDamage()
	{
		return _meleeDamage * _diffDamageBuff;
	}

	public void SetMeleeDamage(float damage)
	{
		_meleeDamage = damage;
	}

	public float GetProjectileDamage()
	{
		return _projectileDamage * _diffDamageBuff;
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
		return _defense * _diffDefenseBuff;
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

	public void UpdateInverts()
	{
		_invertX = PlayerPrefs.GetFloat("xInvert");
		_invertY = PlayerPrefs.GetFloat("yInvert");
		_diceCheat = PlayerPrefs.GetInt("diceCheat") == 1;
		_throwCheat = PlayerPrefs.GetFloat("speedCheat");
	}

	public Camera GetCamera() { return _camera.GetComponent<Camera>(); }

	public void DelayChickenDinner()
	{
		Invoke(nameof(ChickenDinner), 2.5f);
	}

	public void ChickenDinner()
	{
		HudUI.Ref.HideCanvas();
		_winDecal.SetActive(true);
		Debug.Log(_camera.GetComponent<Camera>());
		_camera.GetComponent<Camera>().enabled = false;
		_deadCamera.enabled = true;
		Invoke(nameof(GoToMenu), 2.5f);
	}

}