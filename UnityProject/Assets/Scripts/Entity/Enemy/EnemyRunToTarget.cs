using System;
using UnityEngine;

public class EnemyRunToTarget : EnemyState
{
    [Serializable]
    public struct EnemyRunToTargetData
    {
        public float movementSpeed;
    }

    private EnemyRunToTargetData _data;
    private Vector2 _targetPosition;

    public EnemyRunToTarget(EnemyController enemyController, ref EnemyRunToTargetData data) : base(enemyController)
    {
        _data = data;
    }

    public override void Execute()
    {
        Vector2 enemyPosition = EnemyController.transform.position;
        if (Vector2.Distance(enemyPosition, _targetPosition) < 0.1f)
        {
            EnemyController.entityRigidbody.velocity = Vector2.zero;
            EnemyController.animator.SetBool("isRunning", false);
            EnemyController.ChangeEnemyState(EnemyStates.EnemyIdleWait);
            return;
        }
        if (EnemyController.isTouchingLeftObject ||
            EnemyController.isTouchingRightObject)
        {
            EnemyController.entityRigidbody.velocity = Vector2.zero;
            EnemyController.animator.SetBool("isRunning", false);
            EnemyController.ChangeEnemyState(EnemyStates.EnemyAttack);
            return;
        }

        EnemyController.animator.SetBool("isRunning", true);
        Vector2 movementDirection = (_targetPosition - enemyPosition).x > 0? Vector2.right : Vector2.left;
        EnemyController.lookingDirection = movementDirection;
        EnemyController.spriteRenderer.flipX = movementDirection == Vector2.left;
        EnemyController.Move(movementDirection, _data.movementSpeed);
    }

    public void SetTarget(Vector2 position)
    {
        Vector2 enemyPosition = EnemyController.transform.position;

        _targetPosition = position;
        _targetPosition.y = enemyPosition.y;
    }
    
    public override void DrawGizmos()
    {
        Gizmos.DrawSphere(_targetPosition, 0.25f);
    }
}
