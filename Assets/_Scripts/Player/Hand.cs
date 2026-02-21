using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
  [Header("Dice Prefabs")]
  [SerializeField] GameObject _d4Prefab;
  [SerializeField] GameObject _d6Prefab;
  [SerializeField] GameObject _d8Prefab;
  [SerializeField] GameObject _d12Prefab;
  [SerializeField] GameObject _d20Prefab;
	Animator _anim;
  GameObject _dieToShoot;

	void Start()
	{
    gameObject.SetActive(false);
		_anim = GetComponent<Animator>();
	}

  void Update()
  {
    //if(_dieToShoot != null && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime>1 && !Animator.IsInTransition(0) )
    if (_dieToShoot != null)
    {
      _dieToShoot.GetComponent<DiceProjectile>().Shoot();
      _dieToShoot = null;
    }
  }

  public void Throw()
  {
    int diceSides = WaveAuthority.PlayerRef.GetAmmo();
    if (diceSides == 0)
    {
      // If there was no dice in bag, do nothing
      return;
    }
    Animate();

    switch (diceSides)
    {
      case 4:
        _dieToShoot = Instantiate(_d4Prefab, transform);
        break;
      case 6:
        _dieToShoot = Instantiate(_d6Prefab, transform);
        break;
      case 8:
        _dieToShoot = Instantiate(_d8Prefab, transform);
        break;
      case 12:
        _dieToShoot = Instantiate(_d12Prefab, transform);
        break;
      case 20:
        _dieToShoot = Instantiate(_d20Prefab, transform);
        break;
    }
  }

	public void Animate()
	{
		_anim.SetTrigger("flick");
	}

}