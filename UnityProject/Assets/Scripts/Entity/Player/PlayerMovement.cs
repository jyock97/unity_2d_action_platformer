using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float runningMultiplier;

    private PlayerController _playerController;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private bool _isRunning;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        Vector2 direction = new Vector2();
        if (!_playerController.isTouchingRightWall && !_playerController.isCrouching && Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
            _playerController.lookingDirection = Vector2.right;
            _spriteRenderer.flipX = false;
        }

        if (!_playerController.isTouchingLeftWall && !_playerController.isCrouching && Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
            _playerController.lookingDirection = Vector2.left;
            _spriteRenderer.flipX = true;
        }

        _isRunning = Input.GetKey(KeyCode.LeftShift);
        
        Move(direction);
    }

    private void Move(Vector2 direction)
    {
        Vector2 newVelocity = direction;
        newVelocity *= speed * (_isRunning? runningMultiplier: 1);
        newVelocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = newVelocity;
        
        _animator.SetBool("isRunning", _isRunning);
        _animator.SetBool("isWalking", Math.Abs(newVelocity.x) > 0.01);
    }
}
