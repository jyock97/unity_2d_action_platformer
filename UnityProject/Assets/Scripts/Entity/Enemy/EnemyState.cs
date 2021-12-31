public abstract class EnemyState
{
    public enum EnemyStates
    {
        EnemyIdleMovement,
        EnemyRunToTarget,
        EnemyAttack,
        EnemyIdleWait
    }
    
    protected readonly EnemyController _EnemyController;

    protected EnemyState(EnemyController enemyController)
    {
        _EnemyController = enemyController;
    }
    
    public abstract void Execute();
    
#if UNITY_EDITOR
    public abstract void DrawGizmos();
#endif
}
