using UnityEngine;

public class CheckInEnemyTask : BaseTask
{
    private Transform enemyTransform;
    private Transform transform;

    public CheckInEnemyTask(Transform self, Transform enemy)
    {
        enemyTransform = enemy;
        transform = self;
    }

    public override Status Execute(Blackboard blackBoard)
    {
        if (enemyTransform == null)
        {
            Debug.LogWarning("None Enemy Transform Setting");
            return Status.Failure;
        }

        float sightRange = 10f;
        float distanceToEnemy = Vector3.Distance(blackBoard.GetValue<Vector3>("SelfPosition"), enemyTransform.position);
    
        if(distanceToEnemy < sightRange)
        {
            blackBoard.SetValue("TargetPosition", enemyTransform.position);
            return Status.Success;
        }
        else
        {
            blackBoard.SetValue("TargetPosition", Vector3.zero);
            return Status.Failure;
        }
    }
}
