using UnityEngine;

public class BossAnimator : EnemyAnimator
{

    public void Defeat()
    {
        _anim.SetBool("isDeafeated", true);
    }

}