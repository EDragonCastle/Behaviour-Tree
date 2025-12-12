using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public Animator animator;
    private bool isJumping;
    private bool isSliding;
    private bool isWalking;
    private bool isRunning;

    Dictionary<string, float> animationClipList;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        animationClipList = new Dictionary<string, float>();

        foreach (var value in clips)
        {
            animationClipList.Add(value.name, value.length);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) && !isSliding)
        {
            animator.SetBool("isWalking", true);
            // 이동 로직
            Debug.Log("Move 진행 중");
            isWalking = true;
        }

        if(Input.GetKeyUp(KeyCode.UpArrow) && !isSliding)
        {
            animator.SetBool("isWalking", false);
            isWalking = false;
        }
            
        if(Input.GetKeyDown(KeyCode.LeftControl) && !isSliding && !isJumping)
        {
            animator.SetBool("isJumping", true);
            // 점프 로직
            Debug.Log("Jump 진행 중");
            isJumping = true;
            StartCoroutine(AnimationJumpingLogic(animationClipList["JUMP00"]));
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isRunning && !isJumping)
        {
            animator.SetBool("isSliding", true);
            // Sliding 기능 로직
            Debug.Log("Sliding 진행 중");
            isSliding = true;
            StartCoroutine(AnimationSlidingLogic(animationClipList["SLIDE00"]));
        }


        if (Input.GetKeyDown(KeyCode.Z) && !isSliding)
        {
            animator.SetBool("isRunning", true);
            // Running 기능 로직
            Debug.Log("Running 진행 중");
            isRunning = true;
        }

        if (Input.GetKeyUp(KeyCode.Z) && !isSliding)
        {
            animator.SetBool("isRunning", false);
            isRunning = false;
        }
    }

    private IEnumerator AnimationJumpingLogic(float maxTime)
    {
        yield return new WaitForSeconds(maxTime);
        animator.SetBool("isJumping", false);
        isJumping = false;
    }

    private IEnumerator AnimationSlidingLogic(float maxTime)
    {
        yield return new WaitForSeconds(maxTime);
        animator.SetBool("isSliding", false);
        isSliding = false;
    }

    // parameter로 bool type을 해도 value type이라 적용이 안된다. 그래서 ref로 value type을 ref type으로 변경할 수 있지만?
    // return type이 IEnumator에는 ref keyword 사용이 불가능하다.. 아쉬워요~
    /* 
    private IEnumerator AnimationLogic(float maxTime, ref bool istest)
    {
        yield return new WaitForSeconds(maxTime);
        animator.SetBool("isJumping", false);
    }
    */
}
