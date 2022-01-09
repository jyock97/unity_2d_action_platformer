using UnityEngine;

public class EnemyController : EntityController
{    
    [SerializeField] protected Weapon.WeaponType weaknessWeapon;

    private StateMachine _stateMachine;

    protected override void Start()
    {
        base.Start();
        
        Physics2D.IgnoreLayerCollision(TagsLayers.EnemyLayerMaskIndex, TagsLayers.EnemyLayerMaskIndex);

        _LeftRightObjectLayerMask = TagsLayers.GroundWallLayerMask | TagsLayers.BoxLayerMask | TagsLayers.PlayerLayerMask;

        _stateMachine = GetComponent<StateMachine>();
    }

    public void Move(Vector2 direction, float speed)
    {
        Vector2 newVelocity = direction;
        newVelocity *= speed;
        newVelocity.y = _Rigidbody.velocity.y;
        _Rigidbody.velocity = newVelocity;
    }

    public void DealDamage(Vector2 origin, Weapon.WeaponType weaponType)
    {
        if (weaponType == weaknessWeapon)
        {
            DealDamage(origin);
        }
    }

    protected override void Die()
    {
        base.Die();
        _Rigidbody.simulated = false;
        _stateMachine.Exit();
    }

    // Used by animation
    protected virtual void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    
    protected override void OnDrawGizmos()
    {
        if (GlobalGizmosController.Enemies)
        {
            Color color = Color.red;
            color.a = 0.5f;
            Gizmos.color = color;
            
            base.OnDrawGizmos();
        }
    }
}
