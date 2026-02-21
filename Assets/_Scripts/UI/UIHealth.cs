using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
  [Header("UI Components")]
  [SerializeField] Image _bar;
  Health health;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {
    health = WaveAuthority.PlayerRef.GetComponent<Health>();
    _bar.fillAmount = health.GetHealth() / health.GetMaxHealth();
  }
}
