using UnityEngine;
using UnityEngine.AI;
public class Boss : Enemy
{

	BossAnimator _bossAnim;

	bool _isDefeated = false;
	int _phase;

	void OnEnable()
	{
		_bossAnim = GetComponent<BossAnimator>();
		_phase = 1;
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

		if (_phase == 1 && _health.GetHealth() <= _health.GetMaxHealth() / 2f)
		{
			// Initiate Phase 2
			_phase = 2;
			SetSwimming();
		}
	}

	public override void DeathEvent()
	{
		// Kill the boss

		_bossAnim.Defeat();

		_isDefeated = true;
		_agent.destination = transform.position;

		//Destroy(this.gameObject);
	}

}