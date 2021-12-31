using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;

    private Vector2 _moveDirection;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _rigidbody.velocity = _moveDirection * speed;
        _spriteRenderer.flipX = _moveDirection == Vector2.left;
        StartCoroutine(DestroyAfterSeconds());
    }

    private IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(lifeTime);
        DestroyBullet();
    }

    public void SetDirection(Vector2 direction)
    {
        _moveDirection = direction;
    }
    
    // Used by an Animation
    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(TagsLayers.InteractableTag) && !other.CompareTag(TagsLayers.BulletTag))
        {
            _rigidbody.velocity = Vector2.zero;
            _collider.enabled = false;
            _animator.SetTrigger("hit");
        }

        if (other.gameObject.CompareTag(TagsLayers.EnemyTag))
        {
            other.GetComponent<EnemyController>().DealDamage(transform.position, Weapon.WeaponType.Range);
        }
    }
}
