public abstract class EnemyState
{
    public enum EnemyStates
    {
        EnemyIdleMovement,
        EnemyRunToTarget,
        EnemyAttack,
        EnemyIdleWait
    }
    
    protected readonly StateMachine _StateMachine;

    protected EnemyState(StateMachine stateMachine)
    {
        _StateMachine = stateMachine;
    }
    
    public abstract void Execute();

#if UNITY_EDITOR
    public abstract void DrawGizmos();
#endif
}
