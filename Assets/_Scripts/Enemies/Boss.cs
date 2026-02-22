using UnityEngine;
using UnityEngine.AI;
public class Boss : Enemy
{

	[Header("Boss Settings")]
	[Tooltip("The percentage health that the boss will spawn a wave of minions")]
	[SerializeField] int _waveIntervals;
	[SerializeField] GameObject _tunaCan;

	BossAnimator _bossAnim;

	bool _isDefeated = false;
	int _phase;
	int _wavesLeft;

	void OnEnable()
	{
		_bossAnim = GetComponent<BossAnimator>();
		_phase = 1;
		_wavesLeft = 100 / _waveIntervals;
	}

	protected override void FixedUpdate()
	{
		if (_isDefeated)
		{
			_agent.baseOffset = 1f;
			return;
		}

		_agent.destination = WaveAuthority.PlayerRef.gameObject.transform.position;

		if (_agent.velocity.magnitude > 0.0001f)
		{
			_bossAnim.SetMoving(true);
		}
		else
		{
			_bossAnim.SetMoving(false);
		}

		CheckForAttack();
		int healthPercent = (int)Mathf.Floor(_health.GetHealth() / _health.GetMaxHealth() * 100);

		if (_phase == 1 && healthPercent <= .5f)
		{
			// Initiate Phase 2
			_phase = 2;
			SetSwimming();
		}

		if (Mathf.Ceil(healthPercent / _waveIntervals) == _wavesLeft)
		{
			_wavesLeft--;
			BossAuthority.Ref.ForceSpawnWave();
		}
	}

	float _bu = 0.0f;
	Vector3 _bstartScale = Vector3.zero;
	Transform _brigTrans;

	public override void DeathEvent()
	{
		// Kill the boss

		_bossAnim.Defeat();

		_isDefeated = true;
		_agent.destination = transform.position;

		_bu = 0f;
		_brigTrans = GetComponentInChildren<Animator>().gameObject.transform;
		_bstartScale = _brigTrans.localScale;
		Invoke(nameof(PlayDeathEffects), .8f);
		//Destroy(this.gameObject);
	}

	void PlayDeathEffects()
	{
		Destroy(GetComponent<Collider>());
		Vector3 newScale = Vector3.Lerp(_bstartScale, Vector3.zero, _bu);
		_brigTrans.localScale = newScale;

		_bu += 0.05f;
		if (_bu >= 1f)
		{
			Destroy(_brigTrans.gameObject);
			GameObject tuna = Instantiate(_tunaCan, transform.position, Quaternion.Euler(Vector3.up));
			Destroy(gameObject, 1f);
			_smokeEffect.Play();
			_deadFishEffect1.Play();
			_deadFishEffect2.Play();
			CallChickenDinner();
		}
		else
		{
			Invoke(nameof(PlayDeathEffects), 0.01f);
		}
	}

	void CallChickenDinner()
	{
		Debug.Log("Calling dinner");
		WaveAuthority.PlayerRef.DelayChickenDinner();
	}

}