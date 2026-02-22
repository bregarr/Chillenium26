using UnityEngine;
using UnityEngine.AI;
public class Boss : Enemy
{

    BossAnimator _anim;

    bool _isDefeated = false;

    void OnEnable()
    {
        _anim = GetComponent<BossAnimator>();
    }

    protected override void FixedUpdate()
    {
        if (!_isDefeated)
        {
            base.FixedUpdate();
        }
    }

    public override void DeathEvent()
    {
        // Kill the boss

        _anim.Defeat();

        _isDefeated = true;
        _agent.destination = transform.position;

        //Destroy(this.gameObject);
    }

}