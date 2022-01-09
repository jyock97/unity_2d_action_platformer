using System;
using UnityEngine;

public class EnemyMeleeAttack : EnemyState
{
    [Serializable]
    public struct EnemyMeleeAttackData
    {
        public EnemyStates exitState;
        public Vector2 attackHitBoxOffset;
        public Vector2 attackHitBoxSize;
    }

    private readonly EnemyMeleeAttackData _data;
    private Bounds _hitBox;
    private bool _isAttacking;

    public EnemyMeleeAttack(StateMachine stateMachine, EnemyMeleeAttackData data) : base(stateMachine)
    {
        _data = data;
        CalculateBounds();
    }

    public override void Execute()
    {
        CalculateBounds();

        if (!_isAttacking)
        {
            _isAttacking = true;
            _StateMachine.animator.SetTrigger("attack");
        }
    }

    public override void Exit() { }

    // Used by an Animation
    public void Attack()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_hitBox.center, _hitBox.size, 0, Vector2.zero, 0, TagsLayers.PlayerLayerMask);

        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<EntityController>().DealDamage(_StateMachine.transform.position);
        }

        _onHitTime = Time.time + 0.25f;
    }

    // Used by an Animation
    public void AttackEnd()
    {
        _isAttacking = false;
        _StateMachine.ChangeEnemyState(_data.exitState);
    }

    private void CalculateBounds()
    {
        Vector2 newOffset = _data.attackHitBoxOffset;
        newOffset.x *= _StateMachine.enemyController.lookingDirection == Vector2.left ? -1 : 1;
        Vector2 newPosition = _StateMachine.transform.position;
        newPosition += newOffset;
        _hitBox.center = newPosition;
        _hitBox.size = _data.attackHitBoxSize;
    }

    private float _onHitTime;
    public override void DrawGizmos()
    {
        CalculateBounds();

        if (GlobalGizmosController.Enemies && GlobalGizmosController.EnemyAttackAlways)
        {
            Gizmos.DrawCube(_hitBox.center, _hitBox.size);
        }

        if (GlobalGizmosController.Enemies && GlobalGizmosController.EnemyAttackOnHit)
        {
            if (Time.time < _onHitTime)
            {
                Gizmos.DrawCube(_hitBox.center, _hitBox.size);
            }
        }
    }
}
