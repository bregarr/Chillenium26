using UnityEngine;

public class CursorLock : MonoBehaviour
{

    public static CursorLock Ref { get; private set; }

    void Start()
    {
        LockCursor();
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