using UnityEngine;
using System.Collections;

public enum DamageType { Blade, Piercing, Force, Energy, Fire, Ice, Poison, Shock}

[System.Serializable]
public class WeaponBlueprint : Item {

    //public string displayName;
    public float baseAttackSpeed;
    public float baseRange;
    public DamageType damageType;
    [Header("Percentage of Material stats")]
    public float weightMod;
    public float attackMod;

    public WeaponBlueprint()
    {
        itemType = ItemType.Blueprint;
        stackSize = 1;
        useable = false;
        consumable = false;
    }
}
