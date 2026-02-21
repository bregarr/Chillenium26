using UnityEngine;

public enum eCharacterType
{
	Player, Bass, Dolphin, Shark
}

public class Character : MonoBehaviour
{

	public eCharacterType type { get; private set; }

	public virtual void DeathEvent()
	{

	}

}