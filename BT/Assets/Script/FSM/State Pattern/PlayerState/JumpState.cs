using UnityEngine;

public class JumpState : ICharaterState
{
    #region ICharacter Interface
    // Enter 초기 설정
    public void EnterState(Player state)
    {
        var animator = state.GetAnimator();
        animator.SetBool("isJumping", true);

        var clipList = state.GetAnimationClipList();
        state.StartStateCoroutine(state.PoseToIdle(clipList["JUMP00"], this));

        state.GetRigidBody().AddForce(Vector3.up * state.jumpForce, ForceMode.VelocityChange);
    }

    // Exit 상태에서 나갈 때 실행
    public void ExitState(Player state)
    {
        var animator = state.GetAnimator();
        animator.SetBool("isJumping", false);
    }

    // key Input 처리 후 다음 상태로 넘어갈 때
    public void HandleInput(Player state)
    {

    }

    // Update에서 매 프레임 실행한다.
    public void ExecuteState(Player state)
    {

    }

    public void FixedUpdateState(Player state)
    {

    }

    #endregion
}
