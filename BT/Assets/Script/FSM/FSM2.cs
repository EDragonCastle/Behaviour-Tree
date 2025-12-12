using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character State를 담은 Enum 형식
/// </summary>
public enum CharacterState
{
    Idle,
    Jump,
    Slide,
    Walk,
    Run,
    End,
}

public class FSM2 : MonoBehaviour
{
    public Animator animator;

    Dictionary<string, float> animationClipList;
    List<string> allStateList;

    private CharacterState currentState;

    private void Awake()
    {
        currentState = CharacterState.Idle;
        animator = this.GetComponent<Animator>();

        Initailize();
    }

    void Update()
    {
        // Jump Key Input
        if(Input.GetKeyDown(KeyCode.LeftControl) && 
            currentState != CharacterState.Slide && 
            currentState != CharacterState.Jump)
        {
            ChangeState(CharacterState.Jump);
            StartCoroutine(AnimationLogic(animationClipList["JUMP00"], CharacterState.Jump));
            return;
        }

        // Slide Key Input
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentState == CharacterState.Run)
        {
            ChangeState(CharacterState.Slide);
            StartCoroutine(AnimationLogic(animationClipList["SLIDE00"], CharacterState.Slide));
            return;
        }

        // Slide 중일 때는 입력 못 받게 한다.
        if (currentState == CharacterState.Slide)
            return;

        // Run, Move, Idle
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.Z))
            ChangeState(CharacterState.Run);
        else if (Input.GetKey(KeyCode.UpArrow))
            ChangeState(CharacterState.Walk);
        else if(currentState != CharacterState.Idle)
            ChangeState(CharacterState.Idle);

        StateMovementLogic();
    }

    /// <summary>
    /// 초기값
    /// </summary>
    private void Initailize()
    {
        // Animation State Parameter를 담았다.
        allStateList = new List<string>();
        allStateList.Add("isWalking");
        allStateList.Add("isRunning");
        allStateList.Add("isJumping");
        allStateList.Add("isSliding");

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        animationClipList = new Dictionary<string, float>();

        // Animator의 모든 Clip을 담는다.
        foreach (var value in clips)
        {
            animationClipList.Add(value.name, value.length);
        }
    }

    /// <summary>
    /// State가 변경될 때 동작하는 Logic
    /// </summary>
    /// <param name="newState">새로운 State</param>
    private void ChangeState(CharacterState newState)
    {
        // 기존 State와 비교해서 같으면 함수를 나간다.
        if (currentState == newState)
            return;

        // State 교체
        currentState = newState;

        // Animation Parameter를 초기화한다.
        ResetAnimationParameter();
        
        // State에 따른 Pose를 변경하도록 Animation 설정을 한다.
        EnumToPose(newState);

        // State에 따라 동작되는 Logic
        if(currentState == CharacterState.Jump)
        {
            Debug.Log("Jump Logic");
        }
        else if(currentState == CharacterState.Slide)
        {
            Debug.Log("Slide Logic");
        }    
    }

    /// <summary>
    /// Animator에 있는 Parameter를 초기화 하는 메서드
    /// </summary>
    private void ResetAnimationParameter()
    {
        // Parameter를 돌면서 초기화 해 준다.
        foreach(var stateName in allStateList)
        {
            animator.SetBool(stateName, false);
        }
    }

    /// <summary>
    /// State에 따라 Animation이 적용되도록 하는 메서드
    /// </summary>
    /// <param name="state">State</param>
    private void EnumToPose(CharacterState state)
    {
        switch(state)
        {
            case CharacterState.Jump:
                animator.SetBool("isJumping", true);
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", true);
                break;
            case CharacterState.Slide:
                animator.SetBool("isSliding", true);
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", true);
                break;
            case CharacterState.Walk:
                animator.SetBool("isWalking", true);
                break;
            case CharacterState.Run:
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", true);
                break;
            case CharacterState.Idle:
                animator.SetBool("isWalking", false);
                break;
        }
    }

    /// <summary>
    /// Animation Logic
    /// </summary>
    /// <param name="maxTime">Animation Clip Length</param>
    /// <param name="state">State</param>
    /// <returns></returns>
    private IEnumerator AnimationLogic(float maxTime, CharacterState state)
    {
        yield return new WaitForSeconds(maxTime);

        if(currentState == state)
            ChangeState(CharacterState.Idle);
    }

    /// <summary>
    /// Animation이 아닌 실제 Object가 움직이는 메서드
    /// </summary>
    private void StateMovementLogic()
    {
        switch(currentState)
        {
            case CharacterState.Walk:
                Debug.Log("Walk Logic");
                break;
            case CharacterState.Run:
                Debug.Log("Run Logic");
                break;
            case CharacterState.Idle:
                Debug.Log("Idle Logic");
                break;
            default:
                break;
        }
    }
}
