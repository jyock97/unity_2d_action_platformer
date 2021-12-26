using UnityEngine;

public class RangeWeapon : Weapon
{
    private readonly PlayerController _playerController;
    private readonly Animator _animator;
    private readonly GameObject _bullet;
    private readonly GameObject _leftBulletSpawn;
    private readonly GameObject _rightBulletSpawn;

    public RangeWeapon(PlayerController playerController, GameObject bullet, GameObject leftBulletSpawn, GameObject rightBulletSpawn)
    {
        Type = WeaponType.Range;
        _playerController = playerController;
        _bullet = bullet;
        _leftBulletSpawn = leftBulletSpawn;
        _rightBulletSpawn = rightBulletSpawn;
    }
    
    public override void Attack()
    {
        Vector2 spawnPosition = _playerController.lookingDirection == Vector2.left ?
            _leftBulletSpawn.transform.position : _rightBulletSpawn.transform.position;

        GameObject go = GameObject.Instantiate(_bullet, spawnPosition, Quaternion.identity);
        go.GetComponent<BulletController>().SetDirection(_playerController.lookingDirection);
    }
}
