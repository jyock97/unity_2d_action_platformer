using System;
using UnityEngine;

public class EnemyRunToTarget : EnemyState
{
    [Serializable]
    public struct EnemyRunToTargetData
    {
        public float movementSpeed;
    }
    
    private readonly EnemyRunToTargetData _data;
    private Vector2 _targetPosition;

    public EnemyRunToTarget(StateMachine stateMachine, EnemyRunToTargetData data) : base(stateMachine)
    {
        _data = data;
    }

    public override void Execute()
    {
        Vector2 enemyPosition = _StateMachine.transform.position;
        if (Vector2.Distance(enemyPosition, _targetPosition) < 0.5f)
        {
            _StateMachine._Rigidbody.velocity = Vector2.zero;
            _StateMachine.animator.SetBool("isRunning", false);
            _StateMachine.ChangeEnemyState(EnemyStates.EnemyIdleWait);
            return;
        }
        if ((_StateMachine.enemyController.isTouchingLeftObject || _StateMachine.enemyController.isTouchingRightObject) &&
            _StateMachine.enemyController.leftRightTouchedObjectTag == TagsLayers.PlayerTag)
        {
            _StateMachine._Rigidbody.velocity = Vector2.zero;
            _StateMachine.animator.SetBool("isRunning", false);
            _StateMachine.ChangeEnemyState(EnemyStates.EnemyAttack);
            return;
        }

        _StateMachine.animator.SetBool("isRunning", true);
        Vector2 movementDirection = (_targetPosition - enemyPosition).x > 0? Vector2.right : Vector2.left;
        _StateMachine.enemyController.lookingDirection = movementDirection;
        _StateMachine.spriteRenderer.flipX = movementDirection == Vector2.left;
        _StateMachine.enemyController.Move(movementDirection, _data.movementSpeed);
    }

    public void SetTarget(Vector2 position)
    {
        Vector2 enemyPosition = _StateMachine.transform.position;

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
