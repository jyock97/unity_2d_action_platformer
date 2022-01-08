using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    private GameController _gameController;
    private PlayerController _playerController;
    private Animator _animator;

    private void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_gameController.currentGameMode != GameMode.Gameplay || _playerController.isDead)
        {
            return;
        }
        
        if (Input.GetKey(KeyCode.LeftControl))
        {
            _playerController.isCrouching = true;
            _animator.SetBool("isCrouching", true);
        }
        else
        {
            _playerController.isCrouching = false;
            _animator.SetBool("isCrouching", false);
        }
    }
}
