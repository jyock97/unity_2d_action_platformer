using UnityEngine;

public class EnemyController : EntityController
{    
    [SerializeField] private Weapon.WeaponType weaknessWeapon;

    protected override void Start()
    {
        base.Start();
        
        Physics2D.IgnoreLayerCollision(TagsLayers.EnemyLayerMaskIndex, TagsLayers.EnemyLayerMaskIndex);

        _LeftRightObjectLayerMask = TagsLayers.GroundWallLayerMask | TagsLayers.BoxLayerMask | TagsLayers.PlayerLayerMask;
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
    }

    // Used by animation
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    
    

#if UNITY_EDITOR
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
#endif
}
