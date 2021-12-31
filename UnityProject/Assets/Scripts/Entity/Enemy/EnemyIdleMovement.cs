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

    private readonly Animator _animator;
    private readonly SpriteRenderer _spriteRenderer;
    private readonly EnemyIdleMovementData _data;
    private Vector2 _movementDirection;

    public EnemyIdleMovement(EnemyController enemyController, Animator animator, SpriteRenderer spriteRenderer, EnemyIdleMovementData data) : base(enemyController)
    {
        _animator = animator;
        _spriteRenderer = spriteRenderer;
        _data = data;
        _movementDirection = _data.startLeft ? Vector2.left : Vector2.right;
    }

    public override void Execute()
    {
        // Exit State
        if (_EnemyController.isPlayerOnSight)
        {
            _animator.SetBool("isMoving", false);
            _EnemyController.ChangeEnemyState(EnemyStates.EnemyRunToTarget);
            return;
        }
        
        _animator.SetBool("isMoving", true);
        _spriteRenderer.flipX = _movementDirection == Vector2.left;
        _EnemyController.lookingDirection = _movementDirection;
        if (_EnemyController.isTouchingLeftObject)
        {
            _movementDirection = Vector2.right;
            _spriteRenderer.flipX = false;
        }
        if (_EnemyController.isTouchingRightObject)
        {
            _movementDirection = Vector2.left;
            _spriteRenderer.flipX = true;
        }
        
        _EnemyController.Move(_movementDirection, _data.movementSpeed);
    }

#if UNITY_EDITOR
    public override void DrawGizmos() { }
#endif
}