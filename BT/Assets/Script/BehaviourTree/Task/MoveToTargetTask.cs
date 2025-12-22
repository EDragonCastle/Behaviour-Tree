using UnityEngine;

public class MoveToTargetTask : BaseTask
{
    private float moveSpeed;
    private float stopDistance;

    private Transform transform;
    private Animator animator;
    private string currentAnimationName;

    private float rotationSpeed = 10f;

    public MoveToTargetTask(Transform self, float _moveSpeed = 3f, float _stopDistance = 1f)
    {
        transform = self;
        moveSpeed = _moveSpeed;
        stopDistance = _stopDistance;
    }

    public override Status Execute(Blackboard blackBoard)
    {
        if(!blackBoard.ContainKey("TargetPosition"))
        {
            PlayAnimation(blackBoard, "WAIT00");
            return Status.Failure;
        }

        Vector3 targetPosition = blackBoard.GetValue<Vector3>("TargetPosition");
        targetPosition.y = transform.position.y;

        float distance = Vector3.Distance(transform.position, targetPosition);

        if(distance <= stopDistance)
        {
            PlayAnimation(blackBoard, "WAIT00");
            return Status.Success;
        }

        Vector3 direction = (targetPosition - transform.position).normalized;

        if(direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
        PlayAnimation(blackBoard, "RUN00_F");

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
