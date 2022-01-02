using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerInventory _playerInventory;
    private RangeWeapon _rangeWeapon;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _rangeWeapon = GetComponent<RangeWeapon>();
        _playerInventory = _playerController.playerInventory;
        _playerInventory.AddWeapon(_rangeWeapon);
    }

    private void Update()
    {
        if (_playerController.isDead)
        {
            return;
        }
        
        if (!_playerController.startPushing && Input.GetMouseButtonDown(0))
        {
            _playerInventory.CurrentWeapon.Attack();
        }

        KeyCode[] keycodes = {KeyCode.Alpha1, KeyCode.Alpha2};
        for (int i = 0; i < keycodes.Length; i++)
        {
            if (Input.GetKeyDown(keycodes[i]))
            {
                _playerInventory.SelectWeapon(i);
            }
        }
    }
}
