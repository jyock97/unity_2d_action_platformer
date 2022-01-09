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

    private readonly EnemyIdleMovementData _data;
    private Vector2 _movementDirection;

    public EnemyIdleMovement(StateMachine stateMachine, EnemyIdleMovementData data) : base(stateMachine)
    {
        _data = data;
        _movementDirection = _data.startLeft ? Vector2.left : Vector2.right;
    }

    public override void Execute()
    {
        // Exit State
        if (_StateMachine.isPlayerOnSight)
        {
            _StateMachine.animator.SetBool("isMoving", false);
            _StateMachine.ChangeEnemyState(EnemyStates.EnemyRunToTarget);
            return;
        }
        
        _StateMachine.animator.SetBool("isMoving", true);
        _StateMachine.spriteRenderer.flipX = _movementDirection == Vector2.left;
        _StateMachine.enemyController.lookingDirection = _movementDirection;
        if (_StateMachine.enemyController.isTouchingLeftObject)
        {
            _movementDirection = Vector2.right;
            _StateMachine.spriteRenderer.flipX = false;
        }
        if (_StateMachine.enemyController.isTouchingRightObject)
        {
            _movementDirection = Vector2.left;
            _StateMachine.spriteRenderer.flipX = true;
        }
        
        _StateMachine.enemyController.Move(_movementDirection, _data.movementSpeed);
    }

    public override void Exit() { }
    
    public override void DrawGizmos() { }
}