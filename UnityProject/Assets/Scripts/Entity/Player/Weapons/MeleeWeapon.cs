using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] private float nextAttackDelay;
    [SerializeField] private Vector2 hitBoxRightOffset;
    [SerializeField] private Vector2 hitBoxSize;

    private Bounds _hitBox;
    private float _lastAttackTime;

    protected override void OnValidate()
    {
        base.OnValidate();
        
        CalculateHitBox();
    }

    protected override void Start()
    {
        base.Start();

        CalculateHitBox();
    }

    private void Update()
    {
        CalculateHitBox();
    }

    public override void Attack()
    {
        if (Time.time > _lastAttackTime)
        {
            _PlayerController.isMeleeAttacking = true;
            _Animator.SetTrigger("meleeAttack");
            _lastAttackTime = Time.time + nextAttackDelay;
        }
    }

    private void CalculateHitBox()
    {
        Vector2 newOffset = hitBoxRightOffset;
        newOffset.x *= _PlayerController.lookingDirection == Vector2.left ? -1 : 1;
        Vector2 newPosition = transform.position;
        newPosition += newOffset;
        _hitBox.center = newPosition;
        _hitBox.size = hitBoxSize;
    }

    // Used by an Animation
    private void MeleeAttack()
    {
        RaycastHit2D []hits = Physics2D.BoxCastAll(_hitBox.center, _hitBox.size, 0, Vector2.zero, 0, TagsLayers.EnemyLayerMask);

        foreach (RaycastHit2D raycastHit2D in hits)
        {
            raycastHit2D.collider.gameObject.GetComponent<EnemyController>().DealDamage(transform.position, WeaponType.Melee);
        }

#if UNITY_EDITOR
        _onHitTime = Time.time + 0.25f;
#endif
    }

    // Used by an Animation
    private void MeleeAttackEnd()
    {
        _PlayerController.isMeleeAttacking = false;
    }


#if UNITY_EDITOR
    private float _onHitTime;
    private void OnDrawGizmos()
    {
        Color c = Color.blue;
        c.a = 0.5f;
        Gizmos.color = c;
        
        if (GlobalGizmosController.Player && GlobalGizmosController.MeleeAttackAlways)
        {
            Gizmos.DrawCube(_hitBox.center, _hitBox.size);
        }

        if (GlobalGizmosController.Player && GlobalGizmosController.MeleeAttackOnHit)
        {
            if (Time.time < _onHitTime)
            {
                Gizmos.DrawCube(_hitBox.center, _hitBox.size);
            }
        }
    }
#endif
}
