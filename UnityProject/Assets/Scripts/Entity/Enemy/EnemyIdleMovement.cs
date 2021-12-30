using System;
using UnityEngine;

public class EnemyIdleMovement : EnemyState
{
    [Serializable]
    public struct EnemyIdleMovementData
    {
        public float movementSpeed;
        public bool startLeft;
    }

    private EnemyIdleMovementData _data;
    private Vector2 _movementDirection;

    public EnemyIdleMovement(EnemyController enemyController, ref EnemyIdleMovementData data) : base(enemyController)
    {
        _data = data;
        _movementDirection = _data.startLeft ? Vector2.left : Vector2.right;
    }

    public override void Execute()
    {
        // Exit State
        if (EnemyController.isPlayerOnSight)
        {
            EnemyController.animator.SetBool("isMoving", false);
            EnemyController.ChangeEnemyState(EnemyStates.EnemyRunToTarget);
            return;
        }
        
        EnemyController.animator.SetBool("isMoving", true);
        EnemyController.spriteRenderer.flipX = _movementDirection == Vector2.left;
        EnemyController.lookingDirection = _movementDirection;
        if (EnemyController.isTouchingLeftObject)
        {
            _movementDirection = Vector2.right;
            EnemyController.spriteRenderer.flipX = false;
        }
        if (EnemyController.isTouchingRightObject)
        {
            _movementDirection = Vector2.left;
            EnemyController.spriteRenderer.flipX = true;
        }
        
        EnemyController.Move(_movementDirection, _data.movementSpeed);
    }

    public override void DrawGizmos() { }
}