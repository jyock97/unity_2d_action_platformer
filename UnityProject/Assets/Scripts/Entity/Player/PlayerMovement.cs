using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float runningMultiplier;
    [SerializeField] private float pushingMultiplier;

    private GameController _gameController;
    private PlayerController _playerController;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private bool _isRunning;
    private bool _isTouchingBox;
    private bool _startPushing;
    private bool _isPushing;
    private bool _pauseAnim;
    private Vector2 _direction;

    private void Start()
    {
        _gameController = FindObjectOfType<GameController>();
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
        
        if (_gameController.currentGameMode != GameMode.Gameplay || _playerController.isDead)
        {
            return;
        }
        
        _isTouchingBox = (_playerController.lookingDirection == Vector2.left && _playerController.isTouchingLeftObject ||
                              _playerController.lookingDirection == Vector2.right && _playerController.isTouchingRightObject) && 
                             _playerController.leftRightTouchedObjectTag == TagsLayers.BoxTag;
        
        _direction = Vector2.zero;
        if (!_playerController.isHurt &&
            (!_playerController.isTouchingRightObject || _isTouchingBox && _playerController.isGrounded) &&
            Input.GetKey(KeyCode.D))
        {
            SetDirection(Vector2.right);
        }

        if (!_playerController.isHurt &&
            (!_playerController.isTouchingLeftObject || _isTouchingBox && _playerController.isGrounded) &&
            Input.GetKey(KeyCode.A))
        {
            SetDirection(Vector2.left);
        }

        _isRunning = Input.GetKey(KeyCode.LeftShift);

        if (!_playerController.isHurt && !_playerController.isDead)
        {
            Move(_direction);
        }

        _playerController.startPushing = _startPushing;
    }

    private void Move(Vector2 direction)
    {
        bool isMoving = direction.x > 0.01 || direction.x < -0.01;
        
        Vector2 newVelocity = direction;
        newVelocity *= speed * (_isPushing? pushingMultiplier : _isRunning? runningMultiplier: 1);
        newVelocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = newVelocity;

        if (_isTouchingBox && _playerController.isGrounded)
        {
            if (isMoving)
            {
                _startPushing = true;
                _isPushing = true;
                _pauseAnim = false;
            }
            else
            {
                _isPushing = false;
            }
            
            if (_startPushing && !_isPushing)
            {
                _pauseAnim = true;
            }
        }
        else
        {
            _startPushing = false;
            _isPushing = false;
            _pauseAnim = false;
        }

        _animator.SetBool("isRunning", _isRunning);
        _animator.SetBool("isWalking", Math.Abs(newVelocity.x) > 0.01);
        _animator.SetBool("isPushing", _startPushing);
        _animator.speed = _pauseAnim ? 0 : 1;
    }
}
