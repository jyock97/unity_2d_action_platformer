using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject leftBulletSpawn;
    [SerializeField] private GameObject rightBulletSpawn;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float animShootingStateWaitTime;
    
    private PlayerController _playerController;
    private Animator _animator;
    private PlayerInventory _playerInventory;
    private bool _isShooting;
    private float _animShootingStopTime;

    
    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
        _playerInventory = _playerController.playerInventory;
        _playerInventory.AddWeapon(new RangeWeapon(_playerController, bullet, leftBulletSpawn, rightBulletSpawn));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_playerInventory.CurrentWeapon.Type == Weapon.WeaponType.Range)
            {
                RangeAttack();
            }

            if (_playerInventory.CurrentWeapon.Type == Weapon.WeaponType.Melee)
            {
                MeleeAttack();
            }

        }

        KeyCode[] keycodes = {KeyCode.Alpha1, KeyCode.Alpha2};
        for (int i = 0; i < keycodes.Length; i++)
        {
            if (Input.GetKeyDown(keycodes[i]))
            {
                _playerInventory.SelectWeapon(i);
            }
        }
        
        if (Time.time > _animShootingStopTime)
        {
            _isShooting = false;
        }
        
        _animator.SetBool("isShooting", _isShooting);
    }

    private void RangeAttack()
    {
        if (!_isShooting)
        {
            // If first shoot, wait for the animator to reposition the offset pivot
            StartCoroutine(EndOfFrameRangeAttack());
        }
        else
        {
            _playerInventory.CurrentWeapon.Attack();
        }

        _animShootingStopTime = Time.time + animShootingStateWaitTime;

        _isShooting = true;
        _animator.SetBool("isShooting", _isShooting);
    }

    private IEnumerator EndOfFrameRangeAttack()
    {
        yield return new WaitForEndOfFrame();
        _playerInventory.CurrentWeapon.Attack();
    }

    private void MeleeAttack()
    {
        
    }
}
