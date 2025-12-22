using UnityEngine;
using System.Collections.Generic;

public class PatrolTask : BaseTask
{
    // Patrol Parameter
    private List<Vector3> patrolPoints;
    private int currentPatrolIndex = 0;
    
    // component
    private Transform transform;
    private Animator animator;

    // animatior
    private string currentAnimationName = "";

    // variance
    private float moveSpeed;
    private float stopDistance;
    private float rotationSpeed = 5f;
    private float rotationLimit = 0.95f;

    public PatrolTask(Transform self, List<Vector3> points, float patrolSpeed = 2f, float _stopDistance = 0.5f)
    {
        transform = self;
        patrolPoints = points;
        moveSpeed = patrolSpeed;
        stopDistance = _stopDistance;

        if (patrolPoints == null || patrolPoints.Count == 0) {
            patrolPoints = new List<Vector3> { transform.position };
        }
    }

    public override Status Execute(Blackboard blackBoard)
    {
        if (blackBoard.GetValue<Vector3>("TargetPosition") != Vector3.zero)
        {
            PlayAnimation(blackBoard, "WAIT00");
            return Status.Failure;
        }

        Vector3 currentPatrolPosition = patrolPoints[currentPatrolIndex];
        Vector3 targetPosition = new Vector3(currentPatrolPosition.x, transform.position.y, currentPatrolPosition.z);
        float distance = Vector3.Distance(transform.position, targetPosition);

        if(distance <= stopDistance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            PlayAnimation(blackBoard, "WALK00_F");
            return Status.Success;
        }

        Vector3 direction = (targetPosition - transform.position).normalized;
        if(direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        float dot = Vector3.Dot(transform.forward, direction);

        if (dot > rotationLimit)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            PlayAnimation(blackBoard, "WALK00_F");
        }
        else
            PlayAnimation(blackBoard, "WAIT00");
           
        return Status.Running;
    }

    private void PlayAnimation(Blackboard blackBoard, string stateName)
    {
        animator = blackBoard.GetValue<Animator>("Animator");

        if (blackBoard.GetValue<string>("AnimationName") == stateName) return;

        animator.CrossFade(stateName, 0.2f);
        blackBoard.SetValue("AnimationName", stateName);
    }
}
