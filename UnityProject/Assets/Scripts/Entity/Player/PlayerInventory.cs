using System;
using System.Collections.Generic;

public class PlayerInventory
{
    public Weapon CurrentWeapon;
    
    private readonly List<Weapon> _weapons;
    private readonly HashSet<String> _doorKeys;

    public PlayerInventory()
    {
        _weapons = new List<Weapon>();
        _doorKeys = new HashSet<string>();
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

    public void AddDoorKey(String doorKey)
    {
        _doorKeys.Add(doorKey);
    }
    
    public bool ContainsDoorKey(String doorKey)
    {
        return _doorKeys.Contains(doorKey);
    }
}
