using System;
using UnityEngine;

public class EnemyAttack : EnemyState
{
    [Serializable]
    public struct EnemyAttackData
    {
        public Vector2 attackHitBoxOffset;
        public Vector2 attackHitBoxSize;
    }

    private EnemyAttackData _data;
    private Bounds _hitBox;
    private bool _isAttacking;

    public EnemyAttack(EnemyController enemyController, EnemyAttackData data) : base(enemyController)
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
            EnemyController.animator.SetTrigger("attack");
        }
    }
    
    // Used by an Animation
    public void Attack()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_hitBox.center, _hitBox.size, 0, Vector2.zero, 0, TagsLayers.PlayerLayerMask);

        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<EntityController>().DealDamage();
        }

#if UNITY_EDITOR
        _onHitTime = Time.time + 0.25f;
#endif
    }

    public void AttackEnd()
    {
        _isAttacking = false;
        EnemyController.ChangeEnemyState(EnemyStates.EnemyIdleWait);
    }

    private void CalculateBounds()
    {
        Vector2 newOffset = _data.attackHitBoxOffset;
        newOffset.x *= EnemyController.lookingDirection == Vector2.left ? -1 : 1;
        Vector2 newPosition = EnemyController.transform.position;
        newPosition += newOffset;
        _hitBox.center = newPosition;
        _hitBox.size = _data.attackHitBoxSize;
    }

#if UNITY_EDITOR
    private float _onHitTime;
#endif
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