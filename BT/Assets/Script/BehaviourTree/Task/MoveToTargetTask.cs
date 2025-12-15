using UnityEngine;
using DG.Tweening;

public class MoveToTargetTask : BaseTask
{
    private float moveSpeed;
    private float stopDistance;

    private Transform transform;
    private bool isMoving = false;

    private Tween currentTween;

    public MoveToTargetTask(Transform self, float _moveSpeed = 3f, float _stopDistance = 1f)
    {
        moveSpeed = _moveSpeed;
        stopDistance = _stopDistance;
        transform = self;
    }

    public override Status Execute(Blackboard blackBoard)
    {
        if(!blackBoard.ContainKey("TargetPosition"))
        {
            currentTween?.Kill();
            isMoving = false;
            return Status.Failure;
        }

        Vector3 targetPosition = blackBoard.GetValue<Vector3>("TargetPosition");

        float distance = Vector3.Distance(transform.position, targetPosition);

        if(distance <= stopDistance)
        {
            currentTween?.Kill();
            isMoving = false;
            return Status.Success;
        }

        if(isMoving)
        {
            return Status.Running;
        }

        float duration = distance / moveSpeed;

        currentTween?.Kill();

        currentTween = transform.DOMove(targetPosition, duration)
                                .SetEase(Ease.Linear)
                                .OnComplete(() => {
                                    isMoving = false;
                                });
        isMoving = true;

        return Status.Running;
    }
}
