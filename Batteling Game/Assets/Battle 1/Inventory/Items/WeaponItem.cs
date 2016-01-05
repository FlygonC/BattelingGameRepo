using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SimpleDamage
{
    public DamageType type;
    public float damage;
    public SimpleDamage(float a_amount, DamageType a_type)
    {
        type = a_type;
        damage = a_amount;
    }
}

[System.Serializable]
public class Damage
{
    public List<SimpleDamage> damages = new List<SimpleDamage>();

    public void AddDamage(float a_amount, DamageType a_type)
    {
        damages.Add(new SimpleDamage(a_amount, a_type));
    }
    public Damage()
    {

    }
}

[System.Serializable]
public class WeaponItem : Item {

    public WeaponBlueprint blueprint;
    public CraftingMaterial material;

    //public string _DisplayName;
    //public string displayName { get { return _DisplayName; } }
    public float _Range;
    public float range { get { return _Range; } }
    public float _AttackSpeed;
    public float attackSpeed { get { return _AttackSpeed; } }
    public float _Weight;
    public float weight { get { return _Weight; } }
    public DamageType _DamageType;
    public DamageType damageType { get { return _DamageType; } }
    public float _AttackDamage;
    public float attackDamage { get { return _AttackDamage; } }

    public Damage _Damage = new Damage();
    public Damage damage { get { return _Damage; } }

    void CraftUpdate()
    {
        itemName = material.itemName + " " + blueprint.itemName;
        _Range = blueprint.baseRange;
        _AttackSpeed = blueprint.baseAttackSpeed;
        _Weight = material.weight * blueprint.weightMod;
        _DamageType = blueprint.damageType;
        _AttackDamage = material.sharpness * blueprint.attackMod;

        Damage temp = new Damage();
        temp.AddDamage(attackDamage, damageType);

        _Damage = temp;

        icon = blueprint.icon;
    }

    public override void UseItem(GameObject a_target)
    {
        //base.UseItem(a_target);
        CraftUpdate();
        if (a_target.GetComponent<Creature>())
        {
            a_target.GetComponent<Creature>().EquipWeapon(this);
            Debug.Log("Equiped Weapon Item");
        }
        else
        {
            Debug.LogError("Target to use WeaponItem is not a Creature!");
        }
    }

    public override string WriteTooltip()
    {
        return base.WriteTooltip() + "\n" + 
            attackDamage + " " + damageType.ToString() + " Damage\n" +
            attackSpeed + " Attack Speed\n" +
            range + " Range\n" +
            weight + " Weight";
    }

    public WeaponItem(WeaponBlueprint a_blue, CraftingMaterial a_craft)
    {
        blueprint = a_blue;
        material = a_craft;
        CraftUpdate();

        itemType = ItemType.Weapon;
        stackSize = 1;
        useable = true;
        consumable = false;
    }

    /*public WeaponItem()
    {

    }*/
}
