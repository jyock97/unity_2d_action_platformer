using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Range, Melee
    }

    protected PlayerController _PlayerController;
    protected Animator _Animator;
    
    protected virtual void OnValidate()
    {
        _PlayerController = GetComponent<PlayerController>();
    }

    protected virtual void Start()
    {
        _PlayerController = GetComponent<PlayerController>();
        _Animator = GetComponent<Animator>();
    }
    
    public abstract void Attack();
}
