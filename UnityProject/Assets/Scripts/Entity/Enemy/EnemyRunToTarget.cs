using System;
using UnityEngine;

public class EnemyRunToTarget : EnemyState
{
    [Serializable]
    public struct EnemyRunToTargetData
    {
        public float movementSpeed;
    }
    
    private readonly Rigidbody2D _rigidbody;
    private readonly Animator _animator;
    private readonly SpriteRenderer _spriteRenderer;
    private readonly EnemyRunToTargetData _data;
    private Vector2 _targetPosition;

    public EnemyRunToTarget(EnemyController enemyController, Rigidbody2D rigidbody, Animator animator,
        SpriteRenderer spriteRenderer, EnemyRunToTargetData data) : base(enemyController)
    {
        _rigidbody = rigidbody;
        _animator = animator;
        _spriteRenderer = spriteRenderer;
        _data = data;
    }

    public override void Execute()
    {
        Vector2 enemyPosition = _EnemyController.transform.position;
        if (Vector2.Distance(enemyPosition, _targetPosition) < 0.5f)
        {
            _rigidbody.velocity = Vector2.zero;
            _animator.SetBool("isRunning", false);
            _EnemyController.ChangeEnemyState(EnemyStates.EnemyIdleWait);
            return;
        }
        if (_EnemyController.isTouchingLeftObject || _EnemyController.isTouchingRightObject &&
            _EnemyController.leftRightTouchedObjectTag == TagsLayers.PlayerTag)
        {
            _rigidbody.velocity = Vector2.zero;
            _animator.SetBool("isRunning", false);
            _EnemyController.ChangeEnemyState(EnemyStates.EnemyAttack);
            return;
        }

        _animator.SetBool("isRunning", true);
        Vector2 movementDirection = (_targetPosition - enemyPosition).x > 0? Vector2.right : Vector2.left;
        _EnemyController.lookingDirection = movementDirection;
        _spriteRenderer.flipX = movementDirection == Vector2.left;
        _EnemyController.Move(movementDirection, _data.movementSpeed);
    }

    public void SetTarget(Vector2 position)
    {
        Vector2 enemyPosition = _EnemyController.transform.position;

        _targetPosition = position;
        _targetPosition.y = enemyPosition.y;
    }

#if UNITY_EDITOR
    public override void DrawGizmos()
    {
        Gizmos.DrawSphere(_targetPosition, 0.25f);
    }
#endif
}
