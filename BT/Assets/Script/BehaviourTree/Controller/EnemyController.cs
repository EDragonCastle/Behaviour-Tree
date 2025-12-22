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
    private Animator animator;

    void Awake()
    {
        BT = this.GetComponent<BehaviourTree>();
        animator = this.GetComponent<Animator>();
        if (BT == null)
        {
            enabled = false;
            return;
        }

        enemyBlackBoard = new Blackboard();
        enemyBlackBoard.SetValue("SelfPosition", transform.position);
        enemyBlackBoard.SetValue("Animator", animator);
        enemyBlackBoard.SetValue("AnimationName", "");

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

        // 적 발견 -> 쫓아가는 Task
        IBehaviour targetEnemySequence = new Sequence(new List<IBehaviour> { checkInEnemy, moveTarget});

        // 적 발견 X -> 순찰
        IBehaviour patrolCheckNode = new Inverter(checkInEnemy);
        IBehaviour patrolNode = new Sequence(new List<IBehaviour> { patrolCheckNode, patrol });

        // root Node
        IBehaviour root = new Selector(new List<IBehaviour> { targetEnemySequence, patrolNode });

        return root;
    }
}
