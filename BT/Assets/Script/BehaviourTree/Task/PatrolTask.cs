using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class PatrolTask : BaseTask
{
    private List<Vector3> patrolPoints;
    private Transform transform;

    private float moveSpeed;
    private float stopDistance;

    private int currentPatrolIndex = 0;
    private bool isPatrolling = false;

    private Tween currentTween;

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
        if(patrolPoints.Count == 0)
        {
            currentTween?.Kill();
            isPatrolling = false;
            return Status.Failure;
        }

        Vector3 targetPosition = patrolPoints[currentPatrolIndex];
        float distance = Vector3.Distance(transform.position, targetPosition);

        if(distance <= stopDistance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            isPatrolling = false;
            return Status.Success;
        }

        if (isPatrolling)
            return Status.Running;

        float duration = distance / moveSpeed;

        currentTween?.Kill();

        currentTween = transform.DOMove(targetPosition, duration)
                                .SetEase(Ease.Linear)
                                .OnStart(() => {
                                    isPatrolling = true;
                                })
                                .OnComplete(() => {
                                    isPatrolling = false;
                                });

        return Status.Running;
    }
}
