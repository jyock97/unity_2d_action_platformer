using UnityEngine;

public class EntityController : MonoBehaviour
{
    public bool isGrounded;
    public bool isTouchingLeftObject;
    public bool isTouchingRightObject;
    public Vector2 lookingDirection;

    [SerializeField] protected int maxLife;
    [SerializeField] protected int life;

    protected LayerMask LeftRightObjectLayerMask;
    
    private Collider2D _collider;
    private Bounds _isGroundedBoundsCheck;
    private Bounds _isTouchingLeftWallCheck;
    private Bounds _isTouchingRightWallCheck;

    protected virtual void OnValidate()
    {
        _collider = GetComponent<BoxCollider2D>();
        CalculateBounds();
    }

    protected virtual void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        CalculateBounds();
        IsGrounded();
        isTouchingLeftRightObjects();
    }
    
    public virtual void DealDamage()
    {
        life--;
        
        if (life < 1)
        {
            Destroy(gameObject);
        }
    }

    private void CalculateBounds()
    {
        // Ground bounds
        Bounds bounds = _collider.bounds;
        Vector2 origin = bounds.min;
        origin.x = bounds.center.x;
        origin.y -= 0.15f;
        Vector2 size = bounds.size;
        size.y = 0.2f;
        _isGroundedBoundsCheck.center = origin;
        _isGroundedBoundsCheck.size = size;
        
        // Left Wall bounds
        origin = bounds.center;
        origin.x -= bounds.extents.x + 0.1f;
        size = bounds.size;
        size.x = 0.1f;
        _isTouchingLeftWallCheck.center = origin;
        _isTouchingLeftWallCheck.size = size;
        
        // Right Wall bounds
        origin = bounds.center;
        origin.x += bounds.extents.x + 0.1f;
        size = bounds.size;
        size.x = 0.1f;
        _isTouchingRightWallCheck.center = origin;
        _isTouchingRightWallCheck.size = size;
    }
    
    private void IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_isGroundedBoundsCheck.center, _isGroundedBoundsCheck.size, 0, Vector2.zero, 0, TagsLayers.GroundWallLayerMask);
        isGrounded = hit.collider != null;
    }

    private void isTouchingLeftRightObjects()
    {
        if (LeftRightObjectLayerMask != null)
        {
            RaycastHit2D hit = Physics2D.BoxCast(_isTouchingLeftWallCheck.center, _isTouchingLeftWallCheck.size, 0, Vector2.zero, 0, LeftRightObjectLayerMask);
            isTouchingLeftObject = hit.collider != null;
            
            hit = Physics2D.BoxCast(_isTouchingRightWallCheck.center, _isTouchingRightWallCheck.size, 0, Vector2.zero, 0, LeftRightObjectLayerMask);
            isTouchingRightObject = hit.collider != null;
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
