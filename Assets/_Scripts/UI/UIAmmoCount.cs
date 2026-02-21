using UnityEngine;
using TMPro;

public class UIAmmoCount : MonoBehaviour
{
  [Header("UI Components")]
  [SerializeField] TextMeshProUGUI _tmp;

  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {
    _tmp.text = WaveAuthority.PlayerRef.GetAmmoCount().ToString();
  }
}