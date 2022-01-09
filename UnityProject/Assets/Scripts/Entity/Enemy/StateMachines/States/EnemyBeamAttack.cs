using System;
using UnityEngine;

public class EnemyBeamAttack : EnemyState 
{
    [Serializable]
    public struct EnemyBeamAttackData
    {
        public EnemyStates exitState;
        public GameObject leftBeam;
        public GameObject rightBeam;
        public float beamDistanceOffset;
        public Vector2 targetPosOffset;
        
        public float targetFollowTime;
    }

    private EnemyBeamAttackData _data;

    private Vector2 targetPosition;
    private bool _isAttacking;
    private float _currentTargetFollowTime;
    private GameObject _currentBeam;
    
    public EnemyBeamAttack(StateMachine stateMachine, EnemyBeamAttackData data) : base(stateMachine)
    {
        _data = data;
    }

    public override void Execute()
    {
        // Exit State
        if (!_StateMachine.isPlayerOnSight)
        {
            AttackEnd();
            return;
        }
        
        if (!_isAttacking)
        {
            _StateMachine.enemyController.lookingDirection = (_StateMachine.player.transform.position - _StateMachine.transform.position).x < 1 ? Vector2.left : Vector2.right;
            _StateMachine.GetComponent<SpriteRenderer>().flipX = _StateMachine.enemyController.lookingDirection == Vector2.left;
            _currentBeam = _StateMachine.enemyController.lookingDirection == Vector2.left ? _data.leftBeam : _data.rightBeam;
            _currentBeam.SetActive(true);
            
            _isAttacking = true;
            _currentTargetFollowTime = Time.time + _data.targetFollowTime;
            _StateMachine.animator.SetTrigger("shootBeam");
        }
     
        if (Time.time < _currentTargetFollowTime)
        {
            Vector2 beamDistanceOffset = (targetPosition - (Vector2) _currentBeam.transform.position).normalized * _data.beamDistanceOffset;

            targetPosition = _StateMachine.player.transform.position;
            targetPosition += _data.targetPosOffset + beamDistanceOffset;
        }
        
        FollowTarget(_data.leftBeam);
        FollowTarget(_data.rightBeam);
    }

    public override void Exit()
    {
        _data.leftBeam.SetActive(false);
        _data.rightBeam.SetActive(false);
    }

    private void FollowTarget(GameObject bean)
    {
        Vector2 beamPosition = bean.transform.position;
        Vector2 offsetTargetPos = targetPosition - beamPosition;

        float angle = Mathf.Atan2(offsetTargetPos.y, offsetTargetPos.x) * Mathf.Rad2Deg;
        bean.transform.rotation = Quaternion.Euler(0, 0, angle);

        float distance = Vector2.Distance(beamPosition, targetPosition);
        SpriteRenderer spriteRenderer = bean.transform.Find("Beam").GetComponent<SpriteRenderer>();
        Vector2 newSize = spriteRenderer.size;
        newSize.x = distance + 1;
        spriteRenderer.size = newSize;
    }
    
    // Used by an Animation
    public void Attack()
    {
        GameObject currentBeam = _StateMachine.enemyController.lookingDirection == Vector2.left ? _data.leftBeam : _data.rightBeam;
        RaycastHit2D hit = Physics2D.Linecast(currentBeam.transform.position, targetPosition, TagsLayers.PlayerLayerMask);

        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<EntityController>().DealDamage(_StateMachine.transform.position);
        }
    }

    // Used by an Animation
    public void AttackEnd()
    {
        _isAttacking = false;
        _data.leftBeam.SetActive(false);
        _data.rightBeam.SetActive(false);
        _StateMachine.ChangeEnemyState(_data.exitState);
    }
    
    public override void DrawGizmos()
    {
        GameObject currentBeam = _StateMachine.enemyController.lookingDirection == Vector2.left ? _data.leftBeam : _data.rightBeam;
        Gizmos.DrawLine(currentBeam.transform.position, targetPosition);
    }
}
