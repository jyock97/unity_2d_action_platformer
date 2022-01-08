using System;
using UnityEngine;

public class EnemyIdleWait : EnemyState
{
    [Serializable]
    public struct EnemyIdleWaitData
    {
        public float waitTime;
    }

    private EnemyIdleWaitData _data;
    private float _waitTime;

    public EnemyIdleWait(StateMachine stateMachine, EnemyIdleWaitData data) : base(stateMachine)
    {
        _data = data;
    }

    public override void Execute()
    {
        if (Time.time > _waitTime)
        {
            _StateMachine.ChangeEnemyState(EnemyStates.EnemyIdleMovement);
        }
    }

    public void SetTime(float currentTime)
    {
        _waitTime = currentTime + _data.waitTime;
    }

#if UNITY_EDITOR
    public override void DrawGizmos() { }
#endif
}
