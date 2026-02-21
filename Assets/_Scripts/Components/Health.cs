using System.Buffers;
using UnityEngine;

public class Health : MonoBehaviour
{

	[Header("Health Statics")]
	[SerializeField] float _maxHealth;

	Character _owner;
	float _currHealth;

	void Start()
	{
		_owner = GetComponent<Character>();
		_currHealth = _maxHealth;
	}

	public void TakeDamage(float amount)
	{
		_currHealth -= amount;
		CheckForDead();
	}

	public void AddHealth(float amount)
	{
		_currHealth += amount;
		CheckForDead();
	}

	void CheckForDead()
	{
		_currHealth = Mathf.Clamp(_currHealth, 0, _maxHealth);
		if (_currHealth == 0f)
		{
			// This guy's dead
			_owner.DeathEvent();
		}
	}

}