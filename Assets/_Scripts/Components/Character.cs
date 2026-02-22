using UnityEngine;

public enum eCharacterType
{
	Player, Bass, Dolphin, Shark
}

public class Character : MonoBehaviour
{

	public eCharacterType type { get; private set; }

	protected Transform GetTransform()
	{
		return transform;
	}

	protected void SetScale(Vector3 newScale)
	{
		transform.localScale = newScale;
	}

	public virtual void DeathEvent()
	{

	}

}