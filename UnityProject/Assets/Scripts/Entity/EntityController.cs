using System;
using System.Collections;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public bool isGrounded;
    public bool isTouchingLeftObject;
    public bool isTouchingRightObject;
    public String leftRightTouchedObjectTag;
    public Vector2 lookingDirection;

    [SerializeField] protected int maxLife;
    [SerializeField] protected int life;
    [SerializeField] protected float invulnerableTime;
    [SerializeField] protected float knockbackForce;

    [HideInInspector] public bool isHurt;
    [HideInInspector] public bool isDead;

    protected LayerMask _LeftRightObjectLayerMask;

    protected Rigidbody2D _Rigidbody;
    protected Collider2D _Collider;
    protected SpriteRenderer _SpriteRenderer;
    protected Animator _Animator;
    
    private Bounds _isGroundedBoundsCheck;
    private Bounds _isTouchingLeftWallCheck;
    private Bounds _isTouchingRightWallCheck;

    protected virtual void OnValidate()
    {
        _Collider = GetComponent<BoxCollider2D>();
        CalculateBounds();
    }

    protected virtual void Start()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
        _Collider = GetComponent<BoxCollider2D>();
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _Animator = GetComponent<Animator>();
        
        life = maxLife;
    }

    protected virtual void Update()
    {
        CalculateBounds();
        IsGrounded();
        IsTouchingLeftRightObjects();
    }

    public int GetMaxLife()
    {
        return maxLife;
    }

    public int GetLife()
    {
        return life;
    }
    
    public void DealDamage(Vector2 damageOrigin)
    {
        if (!isHurt && life > 1)
        {
            Hurt(damageOrigin);
        }
        else if (!isDead && life <= 1)
        {
            Die();
        }
    }

    protected void Hurt(Vector2 damageOrigin)
    {
        life--;
        _Rigidbody.velocity = Vector2.zero;
        _Rigidbody.AddForce(CalculateKnockback(damageOrigin));
        StartCoroutine(SetIsHurt());
        _Animator.SetTrigger("hurt");
    }

    protected virtual void Die()
    {
        life--;
        isDead = true;
        _Animator.SetTrigger("die");
    }
    
    private Vector2 CalculateKnockback(Vector2 damageOrigin)
    {
        Vector2 direction = Vector2.one;
        direction.x = (damageOrigin - (Vector2) transform.position).normalized.x < 0.01 ? 1 : -1;
        return direction * knockbackForce;
    }

    private IEnumerator SetIsHurt()
    {
        isHurt = true;
        yield return new WaitForSeconds(invulnerableTime);
        isHurt = false;
    }

    private void CalculateBounds()
    {
        // Ground bounds
        Bounds bounds = _Collider.bounds;
        Vector2 origin = bounds.min;
        origin.x = bounds.center.x;
        origin.y -= 0.15f;
        Vector2 size = new Vector2(0.3f, 0.1f);
        _isGroundedBoundsCheck.center = origin;
        _isGroundedBoundsCheck.size = size;
        
        // Left Object bounds
        origin = bounds.center;
        origin.x -= bounds.extents.x + 0.1f;
        size = bounds.size;
        size.x = 0.1f;
        _isTouchingLeftWallCheck.center = origin;
        _isTouchingLeftWallCheck.size = size;
        
        // Right Object bounds
        origin = bounds.center;
        origin.x += bounds.extents.x + 0.1f;
        size = bounds.size;
        size.x = 0.1f;
        _isTouchingRightWallCheck.center = origin;
        _isTouchingRightWallCheck.size = size;
    }
    
    private void IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_isGroundedBoundsCheck.center, _isGroundedBoundsCheck.size, 0, Vector2.zero, 0, TagsLayers.GroundWallLayerMask | TagsLayers.BoxLayerMask);
        isGrounded = hit.collider != null;
    }

    private void IsTouchingLeftRightObjects()
    {
        leftRightTouchedObjectTag = "";
        RaycastHit2D hit = Physics2D.BoxCast(_isTouchingLeftWallCheck.center, _isTouchingLeftWallCheck.size, 0, Vector2.zero, 0, _LeftRightObjectLayerMask);
        isTouchingLeftObject = hit.collider != null;
        if (isTouchingLeftObject)
        {
            leftRightTouchedObjectTag = hit.collider.tag;
        }
        
        hit = Physics2D.BoxCast(_isTouchingRightWallCheck.center, _isTouchingRightWallCheck.size, 0, Vector2.zero, 0, _LeftRightObjectLayerMask);
        isTouchingRightObject = hit.collider != null;
        if (isTouchingRightObject)
        {
            leftRightTouchedObjectTag = hit.collider.tag;
        }
    }

    protected virtual void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.DrawWireCube(_isGroundedBoundsCheck.center, _isGroundedBoundsCheck.size);
        }
        else
        {
            Gizmos.DrawCube(_isGroundedBoundsCheck.center, _isGroundedBoundsCheck.size);
        }

        if (isTouchingLeftObject)
        {
            Gizmos.DrawWireCube(_isTouchingLeftWallCheck.center, _isTouchingLeftWallCheck.size);
        }
        else
        {
            Gizmos.DrawCube(_isTouchingLeftWallCheck.center, _isTouchingLeftWallCheck.size);
        }

        if (isTouchingRightObject)
        { 
            Gizmos.DrawWireCube(_isTouchingRightWallCheck.center, _isTouchingRightWallCheck.size);
        }
        else
        {
            Gizmos.DrawCube(_isTouchingRightWallCheck.center, _isTouchingRightWallCheck.size);
        }
    }
}
