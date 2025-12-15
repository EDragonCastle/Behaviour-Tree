using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject targetEnemyObject;

    [SerializeField]
    private float moveSpeed = 3f;

    [SerializeField]
    private float patrolSpeed = 2f;

    [SerializeField]
    private List<Vector3> patrolPoints;

    private BehaviourTree BT;
    private Blackboard enemyBlackBoard;

    void Awake()
    {
        BT = this.GetComponent<BehaviourTree>();
        if (BT == null)
        {
            enabled = false;
            return;
        }

        enemyBlackBoard = new Blackboard();
        enemyBlackBoard.SetValue("SelfPosition", transform.position);

        IBehaviour rootNode = BuildBehaviourTree();

        BT.SetUp(rootNode, enemyBlackBoard);
    }

    void Update()
    {
        if (enemyBlackBoard != null)
            enemyBlackBoard.SetValue("SelfPosition", transform.position);
    }

    private IBehaviour BuildBehaviourTree()
    {
        CheckInEnemyTask checkInEnemy = new CheckInEnemyTask(this.transform, targetEnemyObject.transform);
        MoveToTargetTask moveTarget = new MoveToTargetTask(this.transform, moveSpeed);
        PatrolTask patrol = new PatrolTask(this.transform, patrolPoints, patrolSpeed);

        IBehaviour targetEnemySequence = new Sequence(new List<IBehaviour> { checkInEnemy, moveTarget});
        IBehaviour root = new Selector(new List<IBehaviour> { targetEnemySequence, patrol });

        return root;
    }
}
