/// <summary>
/// [Decorator Node]
/// 자식 노드의 실행 결과를 반전시키는 역할을 수행한다.
/// Success를 반환하면 Failure로, Failure을 반환하면 Success로 변환한다.
/// Running 상태는 반전하지 않고, 그대로 유지한다.
/// </summary>
public class Inverter : BaseDecorator
{
    public Inverter(IBehaviour child) : base(child) { }

    public override Status Execute(Blackboard blackBoard)
    {
        Status result = child.Execute(blackBoard);

        // Success -> Failure
        if(result == Status.Success) return Status.Failure;
        // Failure -> Success
        if(result == Status.Failure) return Status.Success;
        
        // Running은 유지
        return Status.Running;
    }
}
