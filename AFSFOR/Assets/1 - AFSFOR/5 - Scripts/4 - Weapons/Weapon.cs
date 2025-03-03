using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public static bool areWeaponsActive = true;
    public bool isWeaponEquiped = false;
    public bool isWeaponOnLeftHand = false;

    public Sprite weaponIcon;

    public void ToggleActivationWeapons(bool activate)
    {
        areWeaponsActive = activate;
    }
}
