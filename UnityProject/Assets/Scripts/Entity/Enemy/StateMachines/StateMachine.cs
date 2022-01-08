using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public bool isPlayerOnSight;
    
    [HideInInspector] public EnemyController enemyController;
    [HideInInspector] public Rigidbody2D _Rigidbody;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;

    protected Dictionary<EnemyState.EnemyStates, EnemyState> _States;
    protected EnemyState _CurrentState;

    protected void OnValidate()
    {
        Start();
    }

    protected virtual void Start()
    {
        enemyController = GetComponent<EnemyController>();
        _Rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        InitEnemyStates();
    }
    
    protected virtual void Update()
    {
        _CurrentState.Execute();
    }

    public virtual void ChangeEnemyState(EnemyState.EnemyStates stateKey)
    {
        _CurrentState = _States[stateKey];
    }

    protected abstract void InitEnemyStates();

    protected virtual void OnDrawGizmos()
    {
        foreach (KeyValuePair<EnemyState.EnemyStates,EnemyState> keyValuePair in _States)
        {
            keyValuePair.Value.DrawGizmos();
        }
    }
}
