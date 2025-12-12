using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// 실제 Player가 조종하는 클래스다.
/// </summary>
public class Player : MonoBehaviour
{
    // Character Animator
    private Animator animator;
    private ICharaterState currentState;
    private Rigidbody rigidBody;

    [SerializeField]
    private string currentStateName;

    // Animator의 Parameter를 받아온다.
    private Dictionary<string, float> animationClipList;

    // Player Movement Varience;
    public float moveSpeed = 1.0f;
    public float jumpForce = 1.0f;
    public float slideForce = 1.0f;
    public float rotationSpeed = 0.1f;

    // Getter Setter Properties
    public Animator GetAnimator() => animator;
    public Dictionary<string, float> GetAnimationClipList() => animationClipList;
    public Rigidbody GetRigidBody() => rigidBody;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        rigidBody = this.GetComponent<Rigidbody>();
        Initalize();
        TransitionState(new IdleState());
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.HandleInput(this);
            currentState.ExecuteState(this);
        }
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.FixedUpdateState(this);
        }
    }

    private void Initalize()
    {
        if (animator == null)
        {
            Debug.LogError("This Object Animator Null Error");
            return;
        }

        // Animator의 모든 Clip의 길이를 담는다.
        var clips = animator.runtimeAnimatorController.animationClips;
        animationClipList = new Dictionary<string, float>();

        foreach (var value in clips)
        {
            animationClipList.Add(value.name, value.length);
        }
    }

    /// <summary>
    /// State를 교체하기 위한 메서드
    /// </summary>
    /// <param name="newState">바꾸고 싶은 State</param>
    public void TransitionState(ICharaterState newState)
    {
        // 비어 있지 않다면 Exit에 한번 갔다온다.
        if(currentState != null)
        {
            currentState.ExitState(this);
        }

        // 새로 State를 교체하고 EnterState를 실행한다.
        currentState = newState;
        currentState.EnterState(this);
        currentStateName = currentState.GetType().Name;
    }

    /// <summary>
    /// State 내부에서 using System.Collection을 사용하지 않게 하기 위해 만든 메서드
    /// </summary>
    /// <param name="coroutine"></param>
    public void StartStateCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// Idle Animator로 되돌릴 메서드
    /// </summary>
    /// <param name="maxTime">Animation Length</param>
    /// <param name="state">현재 State</param>
    /// <returns></returns>
    public IEnumerator PoseToIdle(float maxTime, ICharaterState state)
    {
        yield return new WaitForSeconds(maxTime);

        if (currentState == state)
            TransitionState(new IdleState());
    }

    /// <summary>
    /// Move와 관련된 함수
    /// </summary>
    /// <param name="speed">forward Speed</param>
    public void MoveMent(float speed)
    {
        if(rigidBody != null)
        {
            Vector3 targetVelocity = transform.forward * speed;
            rigidBody.velocity = new Vector3(targetVelocity.x, rigidBody.velocity.y, targetVelocity.z);
        }
    }

    /// <summary>
    /// Move와 관련된 함수
    /// </summary>
    /// <param name="targetVector">Vector3</param>
    public void MoveMent(Vector3 targetVector)
    {
        rigidBody.velocity = new Vector3(targetVector.x, rigidBody.velocity.y, targetVector.z);
    }

    /// <summary>
    /// Player 회전
    /// </summary>
    /// <param name="targetDirection"></param>
    public void PlayerRotate(Vector3 targetDirection)
    {
        if (targetDirection.magnitude < 0.1f)
        {
            transform.DOKill(true);
            return;
        }

        Vector3 horizonDirection = new Vector3(targetDirection.x, 0, targetDirection.z).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(horizonDirection);
        transform.DORotateQuaternion(targetRotation, rotationSpeed).SetEase(Ease.OutSine);
    }
}

/* 
    해당 정보를 사용하려면 조건이 필요하다.
    Parameter를 가져오는 건데 필요할까? 일단 보류
    이게 Animator의 이름을 가져오기 위해 사용하는 거긴 하다.
    근데 또 뭐가 필요한지 어떻게 알까?
    isJumping이 필요하다고 하면 "isJumping"을 직접 찾아야 한다.
    그래서 이 밑에 있는 기능은 필요 없는 기능인 것 같지만 활용할 수 있는 방법은 
    어딘가 저장되어 있는 FSM에서 Animation 정보를 알고 있다면 이 밑에 animatorList를 찾을 수 있게 된다.
    
    private Dictionary<string, AnimatorControllerParameterType> animatorList;
    
    // Animator에 저장되어 있는 Parameter를 가져온다.
    var parameters = animator.parameters;
    foreach(var parameter in parameters)
    {
        string name = parameter.name;
        var type = parameter.type;
        animatorList = new Dictionary<string, AnimatorControllerParameterType>();

        switch(type)
        {
            case AnimatorControllerParameterType.Float:
                animatorList.Add(name, AnimatorControllerParameterType.Float);
                break;
            case AnimatorControllerParameterType.Int:
                animatorList.Add(name, AnimatorControllerParameterType.Int);
                break;
            case AnimatorControllerParameterType.Bool:
                animatorList.Add(name, AnimatorControllerParameterType.Bool);
                break;
            case AnimatorControllerParameterType.Trigger:
                animatorList.Add(name, AnimatorControllerParameterType.Trigger);
                break;
        }
    }
*/