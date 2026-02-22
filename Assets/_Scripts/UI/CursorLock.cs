using UnityEngine;

public class CursorLock : MonoBehaviour
{

	public static CursorLock Ref { get; private set; }

	public bool _lockCursorOnStart = true;

	void Start()
	{
		if (_lockCursorOnStart) LockCursor();
		else UnlockCursor();
		if (Ref)
		{
			Debug.LogWarning("Two cursor lockers in the scene!");
		}
		Ref = this;
	}

	public void LockCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void UnlockCursor()
	{
		Cursor.lockState = CursorLockMode.Confined;
	}

}