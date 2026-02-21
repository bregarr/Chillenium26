using System.Buffers;
using UnityEngine;

public class Health : MonoBehaviour
{

	static Material HitMat;

	[Header("Health Statics")]
	[SerializeField] float _maxHealth;
	[SerializeField] Material _hitMat;
	[SerializeField] float _hitFlashTime;

	Character _owner;
	float _currHealth;
	MeshRenderer _renderer;
	Material _defaultMat;

	void Start()
	{
		_owner = GetComponent<Character>();
		_currHealth = _maxHealth;

		// Quick way of getting the material to all health components
		if (!HitMat && _hitMat)
		{
			HitMat = _hitMat;
		}

		_renderer = GetComponent<MeshRenderer>();
		if (_renderer)
		{
			_defaultMat = _renderer.material;
		}
	}

	public void TakeDamage(float amount)
	{
    if (_owner.gameObject.GetComponent<PlayerControl>())
    {
      _currHealth -= amount / _owner.gameObject.GetComponent<PlayerControl>().GetDefense();
    }
    else
    {
      _currHealth -= amount;
    }

		if (_renderer)
		{
			Material tempMat = _renderer.material;
			_renderer.material = HitMat;
			Invoke(nameof(TimedMaterialReset), _hitFlashTime);
		}

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

	void TimedMaterialReset()
	{
		_renderer.material = _defaultMat;
	}

}