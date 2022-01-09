using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public bool isPlayerOnSight;
    
    [HideInInspector] public GameObject player;
    [HideInInspector] public EnemyController enemyController;
    [HideInInspector] public Rigidbody2D pRigidbody;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;

    protected Dictionary<EnemyState.EnemyStates, EnemyState> _States;
    
    protected EnemyState.EnemyStates _CurrentStateType;
    protected EnemyState _CurrentState;

    protected virtual void OnValidate()
    {
        Start();
    }

    protected virtual void Start()
    {
        player = GameObject.FindWithTag(TagsLayers.PlayerTag);
        enemyController = GetComponent<EnemyController>();
        pRigidbody = GetComponent<Rigidbody2D>();
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
        _CurrentState.Exit();
        _CurrentStateType = stateKey;
        _CurrentState = _States[stateKey];
        switch (stateKey)
        {
            case EnemyState.EnemyStates.EnemyRunToTarget:
                ((EnemyRunToTarget) _States[EnemyState.EnemyStates.EnemyRunToTarget]).SetTarget(player.transform.position);
                break;
            case EnemyState.EnemyStates.EnemyIdleWait:
                ((EnemyIdleWait) _States[EnemyState.EnemyStates.EnemyIdleWait]).SetTime(Time.time);
                break;
        }
    }

    public virtual void Exit()
    {
        _CurrentState.Exit();
    }

    protected abstract void InitEnemyStates();

    protected virtual void OnDrawGizmos()
    {
        if (GlobalGizmosController.AllEnemyStates)
        {
            foreach (KeyValuePair<EnemyState.EnemyStates,EnemyState> keyValuePair in _States)
            {
                keyValuePair.Value.DrawGizmos();
            }
        }
        else
        {
            _CurrentState.DrawGizmos();
        }
    }
}
