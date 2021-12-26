public abstract class Weapon
{
    public enum WeaponType
    {
        Range, Melee
    }

    public WeaponType Type;
    public abstract void Attack();
}
