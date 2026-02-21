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

	[Header("Enemy Options")]
	[SerializeField] eBuffType _dropType = eBuffType.None;
	[SerializeField] bool _canDropDice = true;
	[SerializeField] float _diceDropChance = .5f;

	NavMeshAgent _agent;
	float _lastAttackTime;
	Health _health;

	void Start()
	{
		_agent = GetComponent<NavMeshAgent>();
		_agent.speed = _moveSpeed;
		_agent.stoppingDistance = _attackDistance;
		_lastAttackTime = Time.time;
		_health = GetComponent<Health>();

		if (_dropType == eBuffType.None)
		{
			_canDropDice = false;
		}
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
		if (_canDropDice && Random.Range(0, 1) <= _diceDropChance)
		{
			GameObject dropDiceGO = new GameObject();
			dropDiceGO.AddComponent<Dice>().InitializeDice(_dropType);
			dropDiceGO.transform.position = transform.position;
			dropDiceGO.GetComponent<Collider>().isTrigger = true;
		}
		Destroy(this.gameObject);
	}

	bool AgentInRange()
	{
		return (_agent.destination - transform.position).magnitude <= _attackDistance;
	}

	void OnTriggerEnter(Collider col)
	{
		col.gameObject.TryGetComponent<Sword>(out var colSword);
		if (!colSword)
		{
			// If the enemy didn't collide with a sword
			return;
		}

		// Damage the enemy
		float takeDamage = WaveAuthority.PlayerRef.GetMeleeDamage();
		_health.TakeDamage(takeDamage);

	}

}