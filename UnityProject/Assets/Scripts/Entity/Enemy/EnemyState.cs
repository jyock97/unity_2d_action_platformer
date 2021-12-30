public abstract class EnemyState
{
    public enum EnemyStates
    {
        EnemyIdleMovement,
        EnemyRunToTarget,
        EnemyAttack,
        EnemyIdleWait
    }
    
    protected readonly EnemyController EnemyController;

    protected EnemyState(EnemyController enemyController)
    {
        EnemyController = enemyController;
    }
    
    public abstract void Execute();
    
    public abstract void DrawGizmos();
}
