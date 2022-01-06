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
        if (UIController.UIActive || _playerController.isDead)
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
