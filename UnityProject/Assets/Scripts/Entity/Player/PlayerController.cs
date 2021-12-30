using UnityEngine;

public class PlayerController : EntityController
{
    public bool isCrouching;
    public bool isMeleeAttacking;
    public PlayerInventory playerInventory;

    private Animator _animator;

    private void Awake()
    {
        playerInventory = new PlayerInventory();
    }

    protected override void Start()
    {
        base.Start();
        
        _animator = GetComponent<Animator>();
        
        lookingDirection = Vector2.right;
        LeftRightObjectLayerMask = TagsLayers.GroundWallLayerMask;
    }

    protected override void Update()
    {
        base.Update();
        
        _animator.SetBool("isGrounded", isGrounded);
    }


    protected override void OnDrawGizmos()
    {
        Color c = Color.blue;
        c.a = 0.5f;
        Gizmos.color = c;
        base.OnDrawGizmos();
    }
}
