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
			Destroy(gameObject);
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
}
