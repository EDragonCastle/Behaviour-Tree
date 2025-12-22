/// <summary>
/// [Decorator Node]
/// 자식 노드를 특정 횟수만큼 or 무한히 반복해서 실행한다.
/// 자식이 결과를 내더라도 설정된 반복 횟수에 도달할 때까지 계속 Running을 반환하여 루프를 생성한다.
/// 모든 반복이 완료되면 최종적으로 Success를 반환한다.
/// </summary>
public class Repeater : BaseDecorator
{
    private int repeatCount;
    private int currentCount = 0;
    
    /// <param name="child">반복 실행할 자식 노드</param>
    /// <param name="count">반복 횟수</param>
    public Repeater(IBehaviour child, int count = -1) : base(child)
    {
        repeatCount = count;
    }

    public override Status Execute(Blackboard blackBoard)
    {
        Status childStatus = child.Execute(blackBoard);

        if (childStatus == Status.Running)
            return Status.Running;

        currentCount++;

        if (repeatCount == -1 || currentCount < repeatCount)
            return Status.Running;

        currentCount = 0;
        return Status.Success;
    }
}
