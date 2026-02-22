using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
	[Header("Ammo Statics")]
	[SerializeField] protected float speed;
	[SerializeField] protected float lifeSpan;
	protected bool alreadyHit;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	protected virtual void Start()
	{
		alreadyHit = false;
		GetComponent<Collider>().isTrigger = false;
	}

	// Update is called once per frame
	void Update()
	{
		lifeSpan -= Time.deltaTime;
		if (lifeSpan <= 0)
		{
			Invoke(nameof(ShrinkProjectile), .1f);
			lifeSpan = int.MaxValue;
		}
	}

	public virtual void InitializeProjectile(float speed, float lifeSpan)
	{
		this.speed = speed;
		this.lifeSpan = lifeSpan;
	}

	public virtual void Shoot()
	{

	}

	protected virtual void OnCollisionEnter(Collision other)
	{

	}

	float _u = 0f;
	Vector3 _startScale = Vector3.zero;

	void ShrinkProjectile()
	{
		if (_startScale == Vector3.zero)
		{
			_startScale = transform.localScale;
		}

		Vector3 currScale = Vector3.zero;
		currScale = Vector3.Lerp(_startScale, Vector3.zero, _u * _u);
		transform.localScale = currScale;

		if (_u * _u == 1.0f)
		{
			Destroy(gameObject);
		}
		else
		{
			_u += .1f;
			Invoke(nameof(ShrinkProjectile), .05f);
		}

	}
}
