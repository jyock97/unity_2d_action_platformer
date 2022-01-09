using System.Collections.Generic;
using UnityEngine;

public class Boss01StateMachine : StateMachine
{
    [SerializeField] private GameObject fightArea;
    [SerializeField] private int fightAreaPoints;
    [SerializeField] private float fightAreaRadius;
    [SerializeField] private bool recalculatePoints;
    [SerializeField] private GameObject bossHealthBar;

    [SerializeField] private float changeStateTime;
    
    [Header("EnemyRunToTargetData")]
    [SerializeField] private EnemyRunToTarget.EnemyRunToTargetData enemyRunToTargetData;
    [Header("EnemyIdleWaitData")]
    [SerializeField] private EnemyIdleWait.EnemyIdleWaitData enemyIdleWaitData;
    [Header("EnemyMeleeAttackData")]
    [SerializeField] private EnemyMeleeAttack.EnemyMeleeAttackData enemyMeleeAttackData;
    [Header("EnemyBeamAttack")]
    [SerializeField] private EnemyBeamAttack.EnemyBeamAttackData enemyBeamAttackData;

    private PolygonCollider2D _fightAreaCollider;
    private float _currentChangeStateTime;

    protected override void OnValidate()
    {
        base.OnValidate();

        _fightAreaCollider = fightArea.GetComponent<PolygonCollider2D>();
        if (recalculatePoints)
        {
            CalculateFightAreaPoints();
            recalculatePoints = false;
        }
    }
    
    protected override void Start()
    {
        base.Start();
        
        _fightAreaCollider = fightArea.GetComponent<PolygonCollider2D>();
    }
    
    protected override void Update()
    {
        if (enemyController.isDead)
        {
            return;
        }
        
        base.Update();

        PlayerOnSight();

        if (isPlayerOnSight)
        {
            bossHealthBar.SetActive(true);
        }
        else
        {
            bossHealthBar.SetActive(false);
        }

        if (Time.time > _currentChangeStateTime)
        {
            _currentChangeStateTime = Time.time + changeStateTime;
            
            if (_CurrentStateType == EnemyState.EnemyStates.EnemyRunToTarget || _CurrentStateType == EnemyState.EnemyStates.EnemyIdleWait)
            {
                int rValue = Random.Range(0, 3);
                if (rValue < 1)
                {
                    ChangeEnemyState(EnemyState.EnemyStates.EnemyIdleWait);
                } else if (rValue < 2)
                {
                    ChangeEnemyState(EnemyState.EnemyStates.EnemyRunToTarget);
                }
                else
                {
                    if (Vector2.Distance(transform.position, player.transform.position) < 4)
                    {
                        ChangeEnemyState(EnemyState.EnemyStates.EnemyMeleeAttack);
                    }
                    else
                    {
                        ChangeEnemyState(EnemyState.EnemyStates.EnemyBeamAttack);
                    }
                }
            }
        }
    }

    public override void ChangeEnemyState(EnemyState.EnemyStates stateKey)
    {
        if (_CurrentStateType != stateKey)
        {
            base.ChangeEnemyState(stateKey);
        }
    }
    
    // Used by animation
    private void Attack()
    {
        switch (_CurrentStateType)
        {
            case EnemyState.EnemyStates.EnemyMeleeAttack:
                ((EnemyMeleeAttack) _CurrentState).Attack();
                break;
            case EnemyState.EnemyStates.EnemyBeamAttack:
                ((EnemyBeamAttack) _CurrentState).Attack();
                break;
        }
    }

    // Used by animation
    private void AttackEnd()
    {
        switch (_CurrentStateType)
        {
            case EnemyState.EnemyStates.EnemyMeleeAttack:
                ((EnemyMeleeAttack) _CurrentState).AttackEnd();
                break;
            case EnemyState.EnemyStates.EnemyBeamAttack:
                ((EnemyBeamAttack) _CurrentState).AttackEnd();
                break;
        }
    }
    
    protected override void InitEnemyStates()
    {
        _States = new Dictionary<EnemyState.EnemyStates, EnemyState>
        {
            {EnemyState.EnemyStates.EnemyIdleWait, new EnemyIdleWait(this, enemyIdleWaitData)},
            {EnemyState.EnemyStates.EnemyRunToTarget, new EnemyRunToTarget(this, enemyRunToTargetData)},
            {EnemyState.EnemyStates.EnemyMeleeAttack, new EnemyMeleeAttack(this, enemyMeleeAttackData)},
            {EnemyState.EnemyStates.EnemyBeamAttack, new EnemyBeamAttack(this, enemyBeamAttackData)},
        };
        _CurrentState = _States[EnemyState.EnemyStates.EnemyIdleWait];
    }


    private void CalculateFightAreaPoints()
    {
        Vector2[] points = new Vector2[fightAreaPoints + 1];

        float angle = 0;
        float inc = 180f / fightAreaPoints;
        for (int i = 0; i < fightAreaPoints; i++)
        {
            points[i] = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * fightAreaRadius, Mathf.Sin(angle * Mathf.Deg2Rad) * fightAreaRadius);
            angle += inc;
        }
        points[fightAreaPoints] = Vector2.left * fightAreaRadius;
        
        _fightAreaCollider.points = points;
    }
    
    private void PlayerOnSight()
    {
        ContactFilter2D filter2D = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = TagsLayers.PlayerLayerMask
        };

        RaycastHit2D[] results = new RaycastHit2D[1];
        _fightAreaCollider.Cast(Vector2.zero, filter2D, results);
        isPlayerOnSight = results[0].collider != null && results[0].collider.CompareTag(TagsLayers.PlayerTag);
    }
    
    protected override void OnDrawGizmos()
    {
        if (GlobalGizmosController.Enemies)
        {
            Color color = Color.red;
            color.a = 0.5f;
            Gizmos.color = color;
            
            base.OnDrawGizmos();   
            
            if (GlobalGizmosController.BossFightArea)
            {
                Vector2 fightAreaPosition = fightArea.transform.position;
                Vector2 prevPoint = fightAreaPosition;
                foreach (Vector2 point in _fightAreaCollider.points)
                {
                    Gizmos.DrawLine(prevPoint, point + fightAreaPosition);
                    prevPoint = point + fightAreaPosition;
                }
                Gizmos.DrawLine(fightAreaPosition, prevPoint);
            }
        }
        
    }
}
