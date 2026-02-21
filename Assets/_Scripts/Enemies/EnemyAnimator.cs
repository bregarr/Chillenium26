using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{

	[Header("Animators")]
	[SerializeField] Animator _anim;

	public void Hit()
	{
		_anim.SetTrigger("hit");
	}

	public void SetSwimming(bool newSwim)
	{
		_anim.SetBool("activeSwimming", newSwim);
	}

	public void SetMoving(bool newMove)
	{
		_anim.SetBool("isMoving", newMove);
	}

}