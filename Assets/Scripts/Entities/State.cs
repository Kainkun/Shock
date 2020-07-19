public interface State
{
    void Tick();
    void OnEnter();
    void OnExit();
}