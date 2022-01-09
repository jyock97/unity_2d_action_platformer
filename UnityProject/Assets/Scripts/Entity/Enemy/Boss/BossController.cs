using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : EnemyController
{
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject shield;
    [SerializeField] private float changeSetShieldTime;
    [SerializeField] private float thankYouTransitionTime;

    private HealthBarController _healthBarController;
    private bool _wasHit;

    protected override void Start()
    {
        base.Start();
        _healthBarController = healthBar.GetComponent<HealthBarController>();
        
        SetShield(false);
    }

    protected override void Hurt(Vector2 damageOrigin)
    {
        base.Hurt(damageOrigin);
        
        _healthBarController.SetValue(Mathf.InverseLerp(0, maxLife, life));

        if (!_wasHit)
        {
            _wasHit = true;
            StartCoroutine(ChangeSetShieldAfterSeconds(changeSetShieldTime));
        }
    }

    protected override void Die()
    {
        base.Die();
        
        _healthBarController.SetValue(Mathf.InverseLerp(0, maxLife, life));
        SetShield(false);
    }

    protected override void DestroyEnemy()
    {
        StartCoroutine(LoadThankYouScene());
    }

    private IEnumerator LoadThankYouScene()
    {
        yield return new WaitForSeconds(thankYouTransitionTime);
        SceneManager.LoadScene("ThankYou");
    }

    private void SetShield(bool isActive)
    {
        _wasHit = false;
        shield.SetActive(isActive);
        weaknessWeapon = isActive ? Weapon.WeaponType.Melee : Weapon.WeaponType.Range;
    }

    private IEnumerator ChangeSetShieldAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (!isDead)
        {
            SetShield(!shield.activeSelf);
        }
    }
    
    protected override void OnDrawGizmos()
    {
        if (GlobalGizmosController.BossFightArea)
        {
            Color color = Color.red;
            color.a = 0.5f;
            Gizmos.color = color;
            
            base.OnDrawGizmos();
        }
    }
}
