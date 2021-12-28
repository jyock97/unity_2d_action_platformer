using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Range, Melee
    }

    protected PlayerController PlayerController;
    protected Animator Animator;
    
    protected virtual void OnValidate()
    {
        PlayerController = GetComponent<PlayerController>();
    }

    protected virtual void Start()
    {
        PlayerController = GetComponent<PlayerController>();
        Animator = GetComponent<Animator>();
    }
    
    public abstract void Attack();
}
