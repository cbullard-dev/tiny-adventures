public abstract class PlayerBaseState : BaseState
{
    protected PlayerStateMachine PlayerStateMachine = null;

    protected PlayerAnimationBool PlayerAnimationBool = null;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        PlayerStateMachine = stateMachine;
    }
}