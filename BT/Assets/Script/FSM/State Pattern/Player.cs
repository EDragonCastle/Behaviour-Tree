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
    public float slideForce = 3.0f;
    public float rotationSpeed = 0.2f;

    public float nextAnimationSpeed = 0.5f;

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
