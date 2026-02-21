using UnityEngine;

public class DiceProjectile : MonoBehaviour
{
  [Header("Ammo Statics")]
  [SerializeField] float speed;
  [SerializeField] float lifeSpan;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {
    lifeSpan -= Time.deltaTime;
    if (lifeSpan <= 0)
    {
      Destroy(gameObject);
    }
  }

  public void Shoot()
  {
    Rigidbody rb = GetComponent<Rigidbody>();
    Vector3 force = WaveAuthority.PlayerRef.transform.forward * speed;
    rb.AddForce(force, ForceMode.Impulse);
  }

  void OnTriggerEnter (Collider other)
  {
    GameObject enemy = other.gameObject;

    if (enemy.GetComponent<Enemy>())
    {
      enemy.GetComponent<Health>().TakeDamage(WaveAuthority.PlayerRef.GetProjectileDamage());
    }

    Destroy(gameObject);
  }
}
