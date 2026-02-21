using System.Buffers;
using System.Collections.Generic;
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
	List<SkinnedMeshRenderer> _renderers = new();
	List<Material> _defaultMats = new();

	void Start()
	{
		_owner = GetComponent<Character>();
		_currHealth = _maxHealth;

		// Quick way of getting the material to all health components
		if (!HitMat && _hitMat)
		{
			HitMat = _hitMat;
		}

		foreach (Transform child in GetComponentsInChildren<Transform>())
		{
			child.TryGetComponent<SkinnedMeshRenderer>(out var childRenderer);
			if (childRenderer)
			{
				_renderers.Add(childRenderer);
				_defaultMats.Add(childRenderer.material);
			}
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

		foreach (SkinnedMeshRenderer childRenderer in _renderers)
		{
			childRenderer.material = HitMat;
		}
		Invoke(nameof(TimedMaterialReset), _hitFlashTime);

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
		for (int i = 0; i < _renderers.Count; i++)
		{
			SkinnedMeshRenderer childRenderer = _renderers[i];
			childRenderer.material = _defaultMats[i];
		}
	}

}