using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class Enemy : Character
{

	[Header("Enemy Statics")]
	[SerializeField] float _moveSpeed;
	[SerializeField] float _damageAmount;
	[SerializeField] float _attackSpeed;
	[SerializeField] float _attackDistance;

	NavMeshAgent _agent;
	float _lastAttackTime;

	void Start()
	{
		_agent = GetComponent<NavMeshAgent>();
		_agent.speed = _moveSpeed;
		_agent.stoppingDistance = _attackDistance;
		_lastAttackTime = Time.time;
	}

	void FixedUpdate()
	{
		_agent.destination = WaveAuthority.PlayerRef.gameObject.transform.position;

		CheckForAttack();
	}

	void CheckForAttack()
	{
		// If the enemy is within attacking range of the player
		if (AgentInRange())
		{
			// If the enemy can attack
			if (_lastAttackTime + _attackSpeed <= Time.time)
			{
				// Can attack

				return;
			}
			// This is where the enemy is within range but is on cooldown
		}
	}

	public override void DeathEvent()
	{
		// Kill the enemy
		Destroy(this.gameObject);
	}

	bool AgentInRange()
	{
		return (_agent.destination - transform.position).magnitude <= _attackDistance;
	}

}