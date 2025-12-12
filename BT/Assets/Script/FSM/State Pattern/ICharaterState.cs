public interface ICharaterState
{
    // Enter 초기 설정
    public void EnterState(Player state);
    // Exit 상태에서 나갈 때 실행
    public void ExitState(Player state);
    // key Input 처리 후 다음 상태로 넘어갈 때
    public void HandleInput(Player state);
    // Update에서 매 프레임 실행한다.
    public void ExecuteState(Player state);
    // Physics Update에서 매 프레임 실행된다.
    public void FixedUpdateState(Player state);
}

// 여기서 Interface Parameter에 관해서 void 형식이 있고 State를 Parameter에 적용하는 방법이 있다.
// State 내부에 State를 가지고 있으면 void 형식으로 사용할 수 있는데 그런것보다
// 차라리 Parameter로 그냥 알려주는게 나을 것 같다는 생각이 들어서 Paremeter에 적용하기로 했다.
// ex) private StateManager state; 를 다른 State들이 다 알고 있어야 한다.