public class PlayerIdleState : PlayerBaseState
{
    private PlayerStateMachine _stateMachine;
        
        public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public override void StateEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void StateTick(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public override void StateFixedTick(float fixedDeltaTime)
        {
            throw new System.NotImplementedException();
        }

        public override void StateExit()
        {
            throw new System.NotImplementedException();
        }
    }