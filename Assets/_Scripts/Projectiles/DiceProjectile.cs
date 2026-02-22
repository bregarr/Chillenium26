using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DiceProjectile : MonoBehaviour
{
	[Header("Ammo Statics")]
	[SerializeField] float speed;
	[SerializeField] float lifeSpan;
	bool alreadyHit;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		alreadyHit = false;
		GetComponent<Collider>().isTrigger = false;
		gameObject.layer = LayerMask.NameToLayer("Projectile");
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

	public void InitializeProjectile(float speed, float lifeSpan)
	{
		this.speed = speed;
		this.lifeSpan = lifeSpan;
	}

	public void Shoot()
	{
		Rigidbody rb = GetComponent<Rigidbody>();
		Vector3 forceDirection = WaveAuthority.PlayerRef.RightTransform();
		Transform player = WaveAuthority.PlayerRef.transform;

		RaycastHit hit;
		if (Physics.Raycast(player.position + new Vector3(0f, .75f, 0f), transform.forward, out hit, 500f))
		{
			if (Vector3.Distance(hit.point, transform.position) > 0.5)
			{
				forceDirection = (hit.point - transform.position).normalized;
			}
		}

		float pitch = -player.GetComponent<PlayerControl>().GetPitch();

		forceDirection.y = Mathf.Abs(pitch) / pitch * (1f + .1f * Mathf.Abs(pitch)) / 5f;
		if (forceDirection.y < 0)
		{
			forceDirection.y += 0.2f;
			forceDirection.x *= 1.6f;
			forceDirection.z *= 1.6f;
		}

		Vector3 force = forceDirection * speed;
		transform.SetParent(null, true);
		rb.AddForce(force, ForceMode.Impulse);
	}

	void OnCollisionEnter(Collision other)
	{
		GameObject enemy = other.gameObject;

		if (enemy.GetComponent<Enemy>() && !alreadyHit)
		{
			alreadyHit = true;
			enemy.GetComponent<Health>().TakeDamage(WaveAuthority.PlayerRef.GetProjectileDamage());
			GetComponent<Rigidbody>().linearVelocity /= 4;
		}
	}
}
