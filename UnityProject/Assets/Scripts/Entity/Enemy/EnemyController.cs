using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int maxLife;
    [SerializeField] private int life;

    private void Start()
    {
        life = maxLife;
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
