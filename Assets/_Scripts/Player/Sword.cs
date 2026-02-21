using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Sword : MonoBehaviour
{

	Animator _anim;

	void Start()
	{
		_anim = GetComponent<Animator>();
	}

	public void Animate()
	{
		_anim.SetTrigger("swing");
	}

}