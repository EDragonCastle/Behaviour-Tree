using UnityEngine;

public class WalkState : ICharaterState
{
    private float vertical;
    private float horizontal;

    #region ICharacter Interface
    // Enter 초기 설정
    public void EnterState(Player state)
    {
        var animator = state.GetAnimator();
        animator.SetBool("isWalking", true);
    }

    // Exit 상태에서 나갈 때 실행
    public void ExitState(Player state)
    {
        var animator = state.GetAnimator();
        animator.SetBool("isWalking", false);
    }

    // key Input 처리 후 다음 상태로 넘어갈 때
    public void HandleInput(Player state)
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            state.TransitionState(new JumpState());
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            state.TransitionState(new RunState());
        }
        else if(vertical == 0.0f && horizontal == 0.0f)
        {
            state.TransitionState(new IdleState());
        }
    }

    // Update에서 매 프레임 실행한다.
    public void ExecuteState(Player state)
    {

    }

    public void FixedUpdateState(Player state)
    {
        if(vertical != 0.0f || horizontal != 0.0f)
        {
            Vector3 moveDirection = (state.transform.forward * vertical) + (state.transform.right * horizontal);

            if(moveDirection.magnitude > 1)
            {
                moveDirection.Normalize();
            }

            Vector3 direction = new Vector3(horizontal, 0, vertical);

            // 입력이 있을 때만 회전 방향을 계산한다.
            if (direction.magnitude > 0.1f)
            {
                state.PlayerRotate(direction);
            }

            state.MoveMent(state.transform.forward * direction.magnitude * state.moveSpeed);
        }

    }
    #endregion
}
