using UnityEngine;

public class MeleeWeapon : Weapon
{
    public MeleeWeapon()
    {
        Type = WeaponType.Melee;
    }
    
    public override void Attack()
    {
        Debug.Log("Melee Weapon Attack");
    }
}
