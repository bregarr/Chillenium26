using UnityEngine;

public class BassVariety : MonoBehaviour
{
    [SerializeField] public GameObject _bassRig;

    private Material bassMaterial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Color randomColor = Random.ColorHSV(0f, 1f, 0, 0.75f, 1, 1, 1, 1);
        int childrencount = _bassRig.transform.childCount;
        for(int i = 0; i < childrencount; i++)
        {
            Transform child = _bassRig.transform.GetChild(i);
            Renderer x = child.GetComponent<Renderer>();
            if (x != null)
            {
                x.material.SetColor("_Tint", randomColor);
            }
        }
        
        //if (_bassRig == null)
        //{
        //    //bassMaterial = _bassRig.GetComponent<Renderer>().material;
        //    foreach(Renderer x in _bassRig.GetComponentsInChildren<Renderer>()){
        //        Debug.Log(x.name);
        //        x.material.SetColor("_Tint", randomColor);
        //    }
        //}
    }
}
