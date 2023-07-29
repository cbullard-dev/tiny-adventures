public abstract class BaseState
{
    public abstract void StateEnter();
    public abstract void StateTick(float deltaTime);
    public abstract void StateFixedTick(float fixedDeltaTime);
    public abstract void StateExit();
}
