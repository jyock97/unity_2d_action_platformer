using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float runningMultiplier;

    private PlayerController _playerController;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private bool _isRunning;
    private Vector2 _direction;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        void SetDirection(Vector2 direction)
        {
            // When MeleeAttacking, dont move if grounded, but move in the air
            if (_playerController.isGrounded && !_playerController.isMeleeAttacking || !_playerController.isGrounded)
            {
                if (!_playerController.isCrouching)
                {
                    _direction += direction;
                }
                _playerController.lookingDirection = direction;
                _spriteRenderer.flipX = direction == Vector2.left;
            }
        }
        
        _direction = Vector2.zero;
        if (!_playerController.isTouchingRightWall && Input.GetKey(KeyCode.D))
        {
            SetDirection(Vector2.right);
        }

        if (!_playerController.isTouchingLeftWall  && Input.GetKey(KeyCode.A))
        {
            SetDirection(Vector2.left);

        }

        _isRunning = Input.GetKey(KeyCode.LeftShift);
        
        Move(_direction);
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
