using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    private PlayerController _playerController;
    private Animator _animator;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_playerController.isDead)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _playerController.isCrouching = true;
            _animator.SetBool("isCrouching", true);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _playerController.isCrouching = false;
            _animator.SetBool("isCrouching", false);
        }
    }
}
