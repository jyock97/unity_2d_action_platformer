using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce;

    private PlayerController _playerController;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (_playerController.isDead)
        {
            return;
        }
        
        if (!_playerController.isHurt && _playerController.isGrounded && !_playerController.isCrouching && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            JumpRelease();
        }

        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.gravityScale = 2;
        }
    }

    private void Jump()
    {
        Vector2 newVelocity = _rigidbody.velocity;
        newVelocity.y = jumpForce;
        _rigidbody.velocity = newVelocity;
        _rigidbody.gravityScale = 1;

        _animator.SetTrigger("jump");
    }

    private void JumpRelease()
    {
        if (_rigidbody.velocity.y > 0)
        {
            Vector2 newVelocity = _rigidbody.velocity;
            newVelocity.y /= 3;
            _rigidbody.velocity = newVelocity;
        }
        _rigidbody.gravityScale = 2;
    }
}
