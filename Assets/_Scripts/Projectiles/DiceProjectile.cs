using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DiceProjectile : Projectile
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	protected override void Start()
	{
		base.Start();
		foreach (Transform child in GetComponentsInChildren<Transform>())
		{
			child.gameObject.layer = LayerMask.NameToLayer("Projectile");
		}
		gameObject.layer = LayerMask.NameToLayer("Projectile");
	}

	public override void Shoot()
	{
		Vector3 randomRot = Vector3.one;
		randomRot.x = UnityEngine.Random.Range(0f, 360f);
		randomRot.y = UnityEngine.Random.Range(0f, 360f);
		randomRot.z = UnityEngine.Random.Range(0f, 360f);
		transform.localEulerAngles = randomRot;
		Rigidbody rb = GetComponent<Rigidbody>();
		Vector3 forceDirection = WaveAuthority.PlayerRef.RightTransform();
		Transform player = WaveAuthority.PlayerRef.transform;

		// RaycastHit hit;
		// if (Physics.Raycast(player.position + new Vector3(0f, .75f, 0f), transform.forward, out hit, 500f))
		// {
		// 	if (Vector3.Distance(hit.point, transform.position) > 0.5)
		// 	{
		// 		Debug.Log("Youre doing the thing");
		// 		forceDirection = (hit.point - transform.position).normalized;
		// 	}
		// }

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

	protected override void OnCollisionEnter(Collision other)
	{
		GameObject enemy = other.gameObject;

		if (enemy.GetComponent<Enemy>() && !alreadyHit)
		{
			enemy.GetComponent<Health>().TakeDamage(WaveAuthority.PlayerRef.GetProjectileDamage());
			GetComponent<Rigidbody>().linearVelocity /= 4;
		}
    lifeSpan = 0f;
    alreadyHit = true;
	}
}
