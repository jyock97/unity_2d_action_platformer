using System.Collections.Generic;

public class PlayerInventory
{
    public Weapon CurrentWeapon;
    
    private readonly List<Weapon> _weapons;

    public PlayerInventory()
    {
        _weapons = new List<Weapon>();
    }

    public void SelectWeapon(int index)
    {
        if (_weapons.Count > index)
        {
            CurrentWeapon = _weapons[index];
        }
    }

    public void AddWeapon(Weapon weapon)
    {
        _weapons.Add(weapon);
        if (_weapons.Count == 1)
        {
            CurrentWeapon = _weapons[0];
        }
    }
}
