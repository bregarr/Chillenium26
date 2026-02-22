using UnityEngine;

public class EnemyProjectile : Projectile
{

	public void InitializeProjectile(float speed, float lifeSpan, GameObject parent)
	{
		base.InitializeProjectile(speed, lifeSpan);
		_parentEnemy = parent;
		gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
	}

	GameObject _parentEnemy;

	public override void Shoot()
	{
		Rigidbody rb = GetComponent<Rigidbody>();
		Vector3 forceDirection = WaveAuthority.PlayerRef.transform.position - transform.position;
		forceDirection.y += .5f;

		float spread = _parentEnemy.GetComponent<Enemy>().GetSpread();
		forceDirection.x += Random.Range(-spread, spread);
		forceDirection.z += Random.Range(-spread, spread);

		Debug.DrawRay(transform.position, forceDirection, Color.red, 1f);

		Vector3 force = forceDirection * speed;
		transform.SetParent(null, true);
		rb.AddForce(force, ForceMode.Impulse);
	}

	protected override void OnCollisionEnter(Collision other)
	{
		GameObject player = other.gameObject;

		if (player.GetComponent<PlayerControl>() && !alreadyHit)
		{
			player.GetComponent<Health>().TakeDamage(_parentEnemy.GetComponent<Enemy>().GetProjectileDamage());
			GetComponent<Rigidbody>().linearVelocity /= 4;
		}
		alreadyHit = true;
	}

}