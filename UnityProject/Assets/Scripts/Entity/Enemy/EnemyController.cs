using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{
    public bool isPlayerOnSight;
    
    [SerializeField] private Vector2 sightBoxOffset;
    [SerializeField] private Vector2 sightBoxSize;
    [Header("EnemyIdleMovementData")]
    [SerializeField] private EnemyIdleMovement.EnemyIdleMovementData enemyIdleMovementData;
    [Header("EnemyRunToTargetData")]
    [SerializeField] private EnemyRunToTarget.EnemyRunToTargetData enemyRunToTargetData;
    [Header("EnemyIdleWaitData")]
    [SerializeField] private EnemyIdleWait.EnemyIdleWaitData enemyIdleWaitData;
    [Header("EnemyAttackData")]
    [SerializeField] private EnemyAttack.EnemyAttackData enemyAttackData;
    
    [HideInInspector] public Rigidbody2D entityRigidbody;
    [HideInInspector] public BoxCollider2D entityCollider;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;

    private Dictionary<EnemyState.EnemyStates, EnemyState> _states;
    private EnemyState _currentState;
    private Bounds _sightBox;
    private bool _isPlayerOnSightBoxArea;
    private Vector2 _playerPosition;

    protected override void OnValidate()
    {
        base.OnValidate();
        InitEnemyStates();
    }

    protected override void Start()
    {
        base.Start();
        
        entityRigidbody = GetComponent<Rigidbody2D>();
        entityCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        Physics2D.IgnoreLayerCollision(TagsLayers.EnemyLayerMaskIndex, TagsLayers.EnemyLayerMaskIndex);

        LeftRightObjectLayerMask = TagsLayers.GroundWallLayerMask | TagsLayers.PlayerLayerMask;

        InitEnemyStates();
    }

    protected override void Update()
    {
        base.Update();

        PlayerOnSight();
        
        _currentState.Execute();
    }

    public void Move(Vector2 direction, float speed)
    {
        Vector2 newVelocity = direction;
        newVelocity *= speed;
        newVelocity.y = entityRigidbody.velocity.y;
        entityRigidbody.velocity = newVelocity;
    }

    private void Attack()
    {
        ((EnemyAttack) _currentState).Attack();
    }

    private void AttackEnd()
    {
        ((EnemyAttack) _currentState).AttackEnd();
    }

    private void InitEnemyStates()
    {
        _states = new Dictionary<EnemyState.EnemyStates, EnemyState>
        {
            {EnemyState.EnemyStates.EnemyIdleMovement, new EnemyIdleMovement(this, ref enemyIdleMovementData)},
            {EnemyState.EnemyStates.EnemyRunToTarget, new EnemyRunToTarget(this, ref enemyRunToTargetData)},
            {EnemyState.EnemyStates.EnemyAttack, new EnemyAttack(this, enemyAttackData)},
            {EnemyState.EnemyStates.EnemyIdleWait, new EnemyIdleWait(this, enemyIdleWaitData)},
        };
        _currentState = _states[EnemyState.EnemyStates.EnemyIdleMovement];
    }

    public void ChangeEnemyState(EnemyState.EnemyStates stateKey)
    {
        _currentState = _states[stateKey];
        switch (stateKey)
        {
            case EnemyState.EnemyStates.EnemyRunToTarget:
                ((EnemyRunToTarget) _states[EnemyState.EnemyStates.EnemyRunToTarget]).SetTarget(_playerPosition);
                break;
            case EnemyState.EnemyStates.EnemyIdleWait:
                ((EnemyIdleWait) _states[EnemyState.EnemyStates.EnemyIdleWait]).SetTime(Time.time);
                break;
        }
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

    protected override void OnDrawGizmos()
    {
        if (GlobalGizmosController.Enemies)
        {
            Color color = Color.red;
            color.a = 0.5f;
            Gizmos.color = color;
            
            base.OnDrawGizmos();

            CalculateBounds();
            Gizmos.DrawWireCube(_sightBox.center, _sightBox.size);
            if (_isPlayerOnSightBoxArea && isPlayerOnSight)
            {
                Gizmos.DrawLine(transform.position, _playerPosition);
            }
            
            _currentState.DrawGizmos();
        }
    }
}
