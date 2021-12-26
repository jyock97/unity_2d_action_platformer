using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isGrounded;
    public bool isTouchingLeftWall;
    public bool isTouchingRightWall;
    public bool isCrouching;
    public Vector2 lookingDirection;
    public PlayerInventory playerInventory;

    private Collider2D _collider;
    private Animator _animator;
    private Bounds _isGroundedBoundsCheck;
    private Bounds _isTouchingLeftWallCheck;
    private Bounds _isTouchingRightWallCheck;

    private void OnValidate()
    {
        _collider = GetComponent<BoxCollider2D>();
        CalculateBounds();
    }

    private void Awake()
    {
        playerInventory = new PlayerInventory();
    }

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        
        lookingDirection = Vector2.right;
    }

    private void Update()
    {
        CalculateBounds();
        IsGrounded();
        isTouchingWalls();
        
        _animator.SetBool("isGrounded", isGrounded);
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
        RaycastHit2D hit = Physics2D.BoxCast(_isGroundedBoundsCheck.center, _isGroundedBoundsCheck.size, 0, Vector2.down, 0, TagsLayers.GroundWallLayerMask);
        isGrounded = hit.collider != null;
    }

    private void isTouchingWalls()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_isTouchingLeftWallCheck.center, _isTouchingLeftWallCheck.size, 0, Vector2.down, 0, TagsLayers.GroundWallLayerMask);
        isTouchingLeftWall = hit.collider != null;
        
        hit = Physics2D.BoxCast(_isTouchingRightWallCheck.center, _isTouchingRightWallCheck.size, 0, Vector2.down, 0, TagsLayers.GroundWallLayerMask);
        isTouchingRightWall = hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (GlobalGizmosControlelr.Player)
        {
            if (isGrounded)
            {
                Gizmos.DrawWireCube(_isGroundedBoundsCheck.center, _isGroundedBoundsCheck.size);
            }
            else
            {
                Gizmos.DrawCube(_isGroundedBoundsCheck.center, _isGroundedBoundsCheck.size);
            }

            if (isTouchingLeftWall)
            {
                Gizmos.DrawWireCube(_isTouchingLeftWallCheck.center, _isTouchingLeftWallCheck.size);
            }
            else
            {
                Gizmos.DrawCube(_isTouchingLeftWallCheck.center, _isTouchingLeftWallCheck.size);
            }

            if (isTouchingRightWall)
            { 
                Gizmos.DrawWireCube(_isTouchingRightWallCheck.center, _isTouchingRightWallCheck.size);
            }
            else
            {
                Gizmos.DrawCube(_isTouchingRightWallCheck.center, _isTouchingRightWallCheck.size);
            }
        }
    }
}
