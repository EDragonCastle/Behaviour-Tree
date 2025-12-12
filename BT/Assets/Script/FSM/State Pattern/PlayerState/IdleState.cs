using UnityEngine;

public class IdleState : ICharaterState
{
    #region ICharacter Interface
    // Enter 초기 설정
    public void EnterState(Player state)
    {
        var animator = state.GetAnimator();
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }

    // Exit 상태에서 나갈 때 실행
    public void ExitState(Player state)
    {

    }

    // key Input 처리 후 다음 상태로 넘어갈 때
    public void HandleInput(Player state)
    {
        // Run State
        if((Input.GetAxis("Vertical") != 0.0f || Input.GetAxis("Horizontal") != 0.0f) && Input.GetKey(KeyCode.Z))
        {
            state.TransitionState(new RunState());
        }
        else if(Input.GetAxis("Vertical") != 0.0f || Input.GetAxis("Horizontal") != 0.0f)
        {
            state.TransitionState(new WalkState());
        }    

        // Jump State
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            state.TransitionState(new JumpState());
        }
    }

    // Update에서 매 프레임 실행한다.
    public void ExecuteState(Player state)
    {
        Debug.Log("Idle 실행 중");
    }

    public void FixedUpdateState(Player state)
    {

    }
    #endregion

}
