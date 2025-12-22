using UnityEngine;

public class RunState : ICharaterState
{
    private float vertical;
    private float horizontal;

    #region ICharacter Interface
    // Enter 초기 설정
    public void EnterState(Player state)
    {
        var animator = state.GetAnimator();
        
        animator.CrossFade("RUN00_F", state.nextAnimationSpeed);

    }

    // Exit 상태에서 나갈 때 실행
    public void ExitState(Player state)
    {

    }

    // key Input 처리 후 다음 상태로 넘어갈 때
    public void HandleInput(Player state)
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            state.TransitionState(new SlideState());
        }
        else if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            state.TransitionState(new JumpState());
        }
        else if(Input.GetKeyUp(KeyCode.Z))
        {
            state.TransitionState(new WalkState());
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
        if (vertical != 0.0f || horizontal != 0.0f)
        {
            Transform cameraTransform = Camera.main.transform;
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = ((cameraForward * vertical) + (cameraRight * horizontal)).normalized;

            // 입력이 있을 때만 회전 방향을 계산한다.
            if (moveDirection.magnitude > 0.1f)
            {
                state.PlayerRotate(moveDirection);
            }

            state.MoveMent(moveDirection * state.moveSpeed * 2);
        }
    }
    #endregion
}

// horizon, vertical