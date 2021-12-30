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

    public EnemyIdleWait(EnemyController enemyController, EnemyIdleWaitData data) : base(enemyController)
    {
        _data = data;
    }

    public override void Execute()
    {
        if (Time.time > _waitTime)
        {
            EnemyController.ChangeEnemyState(EnemyStates.EnemyIdleMovement);
        }
    }

    public void SetTime(float currentTime)
    {
        _waitTime = currentTime + _data.waitTime;
    }

    public override void DrawGizmos() { }
}
