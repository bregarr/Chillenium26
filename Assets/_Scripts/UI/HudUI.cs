using UnityEngine;

public class HudUI : MonoBehaviour
{

    public static HudUI Ref { get; private set; }

    void Start()
    {
        if (Ref)
        {
            Debug.LogWarning("Two huds!");
        }
        Ref = this;
    }

    public void HideCanvas()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void ShowCanvas()
    {
        GetComponent<Canvas>().enabled = true;
    }

}