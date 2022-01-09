using System;
using UnityEngine;

public class EnemyIdleWait : EnemyState
{
    [Serializable]
    public struct EnemyIdleWaitData
    {
        public EnemyStates exitState;
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
            _StateMachine.ChangeEnemyState(_data.exitState);
        }
    }

    public override void Exit() { }

    public void SetTime(float currentTime)
    {
        _waitTime = currentTime + _data.waitTime;
    }

    public override void DrawGizmos() { }
}
