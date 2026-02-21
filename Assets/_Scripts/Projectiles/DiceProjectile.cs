using UnityEngine;

public class DiceProjectile : MonoBehaviour
{
  [Header("Ammo Statics")]
  [SerializeField] float speed;
  [SerializeField] float lifeSpan;
  bool alreadyHit;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    alreadyHit = false;
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
    Vector3 forceDirection = transform.forward;
    Transform player = WaveAuthority.PlayerRef.transform;

    RaycastHit hit;
    if (Physics.Raycast(player.position + new Vector3(0f, .75f, 0f), transform.forward, out hit, 500f))
    {
      if (Vector3.Distance(hit.point, transform.position) > 0.5)
      {
        forceDirection = (hit.point - transform.position).normalized;
      }
    }

    Vector3 force = forceDirection * speed;
    transform.SetParent(null, true);
    rb.AddForce(force, ForceMode.Impulse);
  }

  void OnCollisionEnter (Collision other)
  {
    GameObject enemy = other.gameObject;

    if (enemy.GetComponent<Enemy>() && !alreadyHit)
    {
      alreadyHit = true;
      enemy.GetComponent<Health>().TakeDamage(WaveAuthority.PlayerRef.GetProjectileDamage());
      GetComponent<Rigidbody>().linearVelocity /= 4;
    }
  }
}
