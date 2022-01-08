using System.Collections.Generic;
using UnityEngine;

public class Enemy01StateMachine : StateMachine
{
    [Header("EnemyIdleMovementData")]
    [SerializeField] private EnemyIdleMovement.EnemyIdleMovementData enemyIdleMovementData;
    [Header("EnemyRunToTargetData")]
    [SerializeField] private EnemyRunToTarget.EnemyRunToTargetData enemyRunToTargetData;
    [Header("EnemyIdleWaitData")]
    [SerializeField] private EnemyIdleWait.EnemyIdleWaitData enemyIdleWaitData;
    [Header("EnemyAttackData")]
    [SerializeField] private EnemyAttack.EnemyAttackData enemyAttackData;
    
    [SerializeField] private Vector2 sightBoxOffset;
    [SerializeField] private Vector2 sightBoxSize;
    

    private Bounds _sightBox;
    private bool _isPlayerOnSightBoxArea;
    private Vector2 _playerPosition;

    protected override void Start()
    {
        base.Start();
        
        InitEnemyStates();
    }
    
    protected override void Update()
    {
        CalculateBounds();
        PlayerOnSight();
        
        base.Update();
    }
    
    private void CalculateBounds()
         {
             Vector2 newPosition = transform.position;
             newPosition += sightBoxOffset;
             _sightBox.center = newPosition;
             _sightBox.size = sightBoxSize;
         }
    
    private void PlayerOnSight()
    {
        if (!enemyController.isGrounded)
        {
            return;
        }
        
        RaycastHit2D hit = Physics2D.BoxCast(_sightBox.center, _sightBox.size, 0, Vector2.zero, 0, TagsLayers.PlayerLayerMask);
        if (hit.collider != null)
        {
            _isPlayerOnSightBoxArea = true;
            _playerPosition = hit.collider.transform.position;
            
            hit = Physics2D.Linecast(_sightBox.center, _playerPosition, ~(TagsLayers.EnemyLayerMask | TagsLayers.InteractableLayerMask));
            isPlayerOnSight = hit.collider != null && hit.collider.gameObject.CompareTag(TagsLayers.PlayerTag);
        }
        else
        {
            _isPlayerOnSightBoxArea = false;
            isPlayerOnSight = false;
        }
    }
    
    protected override void InitEnemyStates()
    {
        _States = new Dictionary<EnemyState.EnemyStates, EnemyState>
        {
            {EnemyState.EnemyStates.EnemyIdleMovement, new EnemyIdleMovement(this, enemyIdleMovementData)},
            {EnemyState.EnemyStates.EnemyRunToTarget, new EnemyRunToTarget(this, enemyRunToTargetData)},
            {EnemyState.EnemyStates.EnemyAttack, new EnemyAttack(this, enemyAttackData)},
            {EnemyState.EnemyStates.EnemyIdleWait, new EnemyIdleWait(this, enemyIdleWaitData)},
        };
        _CurrentState = _States[EnemyState.EnemyStates.EnemyIdleMovement];
    }
    
    public override void ChangeEnemyState(EnemyState.EnemyStates stateKey)
    {
        _CurrentState = _States[stateKey];
        switch (stateKey)
        {
            case EnemyState.EnemyStates.EnemyRunToTarget:
                ((EnemyRunToTarget) _States[EnemyState.EnemyStates.EnemyRunToTarget]).SetTarget(_playerPosition);
                break;
            case EnemyState.EnemyStates.EnemyIdleWait:
                ((EnemyIdleWait) _States[EnemyState.EnemyStates.EnemyIdleWait]).SetTime(Time.time);
                break;
        }
    }
    
    // Used by animation
    private void Attack()
    {
        ((EnemyAttack) _CurrentState).Attack();
    }

    // Used by animation
    private void AttackEnd()
    {
        ((EnemyAttack) _CurrentState).AttackEnd();
    }

    protected override void OnDrawGizmos()
    {
        if (GlobalGizmosController.Enemies)
        {
            Color color = Color.red;
            color.a = 0.5f;
            Gizmos.color = color;
            
            CalculateBounds();
            Gizmos.DrawWireCube(_sightBox.center, _sightBox.size);
            if (_isPlayerOnSightBoxArea && isPlayerOnSight)
            {
                Gizmos.DrawLine(transform.position, _playerPosition);
            }
            
            base.OnDrawGizmos();
        }
    }
}
