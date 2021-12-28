using System.Collections;
using UnityEngine;

public class RangeWeapon : Weapon
{
    [SerializeField] private GameObject leftBulletSpawn;
    [SerializeField] private GameObject rightBulletSpawn;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float animShootingStateWaitTime;
    
    private PlayerInventory _playerInventory;
    private bool _isShooting;
    private float _animShootingStopTime;

    private void Update()
    {
        if (Time.time > _animShootingStopTime)
        {
            _isShooting = false;
        }
        
        Animator.SetBool("isShooting", _isShooting);
    }
    
    public override void Attack()
    {
        if (!_isShooting)
        {
            // If first shoot, wait for the animator to reposition the offset pivot
            StartCoroutine(EndOfFrameRangeAttack());
        }
        else
        {
            Shoot();
        }

        _animShootingStopTime = Time.time + animShootingStateWaitTime;

        _isShooting = true;
        Animator.SetBool("isShooting", _isShooting);
    }

    private void Shoot()
    {
        Vector2 spawnPosition = PlayerController.lookingDirection == Vector2.left ?
        leftBulletSpawn.transform.position : rightBulletSpawn.transform.position;

        GameObject go = Instantiate(bullet, spawnPosition, Quaternion.identity);
        go.GetComponent<BulletController>().SetDirection(PlayerController.lookingDirection);
    }

    private IEnumerator EndOfFrameRangeAttack()
    {
        yield return new WaitForEndOfFrame();
        Shoot();
    }
}
