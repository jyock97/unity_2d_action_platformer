using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : EntityController
{
    public bool isCrouching;
    public bool isMeleeAttacking;
    public PlayerInventory playerInventory;

    private void Awake()
    {
        playerInventory = new PlayerInventory();
    }

    protected override void Start()
    {
        base.Start();

        lookingDirection = Vector2.right;
        _LeftRightObjectLayerMask = TagsLayers.GroundWallLayerMask;
    }

    protected override void Update()
    {
        base.Update();
        
        _Animator.SetBool("isGrounded", isGrounded);
    }

    protected override void Die()
    {
        base.Die();
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("GameOver");
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
