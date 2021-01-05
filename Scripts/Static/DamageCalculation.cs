using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageCalculation
{
    // This script is currently unused

    public static float Damage()
    {
        float damage = 0;
        damage += ArmorAbsorption(0,0);
        return damage;
    }

    public static int ArmorAbsorption(int weaponPenetration, int targetArmor)
    {
        if (weaponPenetration >= targetArmor)
        {
            return 0;
        }
        else
        {
            return targetArmor - weaponPenetration;
        }
    }

    public static void WeaponVulnerability()
    {

    }

    public static void CriticalHit()
    {

    }

    public static void RandomFactor()
    {

    }
}
