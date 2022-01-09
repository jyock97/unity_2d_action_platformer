public abstract class EnemyState
{
    public enum EnemyStates
    {
        EnemyIdleMovement,
        EnemyRunToTarget,
        EnemyMeleeAttack,
        EnemyBeamAttack,
        EnemyIdleWait
    }
    
    protected readonly StateMachine _StateMachine;

    protected EnemyState(StateMachine stateMachine)
    {
        _StateMachine = stateMachine;
    }
    
    public abstract void Execute();
    
    public abstract void Exit();
    
    public abstract void DrawGizmos();
}
