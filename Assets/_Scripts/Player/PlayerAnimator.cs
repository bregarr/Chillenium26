using UnityEngine;

public enum eWalkState
{
    Idle, Walking, Running
}

public class PlayerAnimator : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] Animator _bodyAnim;
    [SerializeField] Animator _swordAnim;

    public void Slash()
    {
        int randomSlash = Random.Range(1, 4);
        switch (randomSlash)
        {
            case 1:
                _bodyAnim.SetTrigger("slash1");
                _swordAnim.SetTrigger("slash1");
                break;
            case 2:
                _bodyAnim.SetTrigger("slash2");
                _swordAnim.SetTrigger("slash2");
                break;
            case 3:
                _bodyAnim.SetTrigger("slash3");
                _swordAnim.SetTrigger("slash3");
                break;
        }
    }

    public void UpdateWalkState(eWalkState state)
    {
        switch (state)
        {
            case eWalkState.Idle:
                _bodyAnim.SetBool("isWalking", false);
                _bodyAnim.SetBool("isRunning", false);
                _swordAnim.SetBool("isWalking", false);
                _swordAnim.SetBool("isRunning", false);
                break;
            case eWalkState.Walking:
                _bodyAnim.SetBool("isWalking", true);
                _bodyAnim.SetBool("isRunning", false);
                _swordAnim.SetBool("isWalking", true);
                _swordAnim.SetBool("isRunning", false);
                break;
            case eWalkState.Running:
                _bodyAnim.SetBool("isWalking", false);
                _bodyAnim.SetBool("isRunning", true);
                _swordAnim.SetBool("isWalking", false);
                _swordAnim.SetBool("isRunning", true);
                break;
        }
    }

}