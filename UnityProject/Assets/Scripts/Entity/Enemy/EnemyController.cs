using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int maxLife;
    [SerializeField] private int life;

    public void Start()
    {
        life = maxLife;
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            DealDamage();
        }
    }

    public void DealDamage()
    {
        life--;
        
        if (life < 1)
        {
            Destroy(gameObject);
        }
    }
}
