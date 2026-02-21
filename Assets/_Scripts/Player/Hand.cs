using UnityEngine;

public class Hand : MonoBehaviour
{
	Animator _anim;

	[Header("Projectile Holder")]
	[SerializeField] GameObject _projectileHolder;

	void Start()
	{
		_anim = GetComponent<Animator>();
	}

	void Update()
	{

	}

	public void Throw()
	{
		int diceSides = WaveAuthority.PlayerRef.GetAmmo();
		if (diceSides == 0)
		{
			// If there was no dice in bag, do nothing
			return;
		}

		GameObject dieToShoot = null;

		switch (diceSides)
		{
			case 4:
				dieToShoot = Instantiate(DiceAuthority.Ref.GetDiceBySides(4), transform.position, transform.rotation, _projectileHolder.transform);
				break;
			case 6:
				dieToShoot = Instantiate(DiceAuthority.Ref.GetDiceBySides(6), transform.position, transform.rotation, _projectileHolder.transform);
				break;
			case 8:
				dieToShoot = Instantiate(DiceAuthority.Ref.GetDiceBySides(8), transform.position, transform.rotation, _projectileHolder.transform);
				break;
			case 12:
				dieToShoot = Instantiate(DiceAuthority.Ref.GetDiceBySides(12), transform.position, transform.rotation, _projectileHolder.transform);
				break;
			case 20:
				dieToShoot = Instantiate(DiceAuthority.Ref.GetDiceBySides(20), transform.position, transform.rotation, _projectileHolder.transform);
				break;
		}

		if (dieToShoot)
		{
			DiceProjectile dieProj = dieToShoot.AddComponent<DiceProjectile>();
			dieProj.InitializeProjectile(WaveAuthority.PlayerRef.GetProjectileSpeed(), WaveAuthority.PlayerRef.GetProjectileTTL());
			dieProj.Shoot();
		}

	}

}