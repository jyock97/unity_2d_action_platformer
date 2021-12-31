using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    protected override void Die()
    {
        base.Die();
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Scenes/TestTilemap");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(TagsLayers.EnemyTag))
        {
            DealDamage(other.gameObject.transform.position);
        }
    }

    protected override void OnDrawGizmos()
    {
        Color c = Color.blue;
        c.a = 0.5f;
        Gizmos.color = c;
        base.OnDrawGizmos();
    }
}
