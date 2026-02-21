using UnityEngine;

public class WaveAuthority : MonoBehaviour
{

	public static WaveAuthority Ref { get; private set; }
	public static PlayerControl PlayerRef { get; private set; }

	void Start()
	{
		if (Ref)
		{
			Debug.LogWarning("There are two wave authorities in the scene!");
		}
		Ref = this;
	}

	public static void SetPlayerRef(PlayerControl newRef)
	{
		PlayerRef = newRef;
	}

}