using UnityEngine;
using System.Collections;

[System.Serializable]
public class EquipmentItem : Item {

    public EquipmentBlueprint blueprint;
    public CraftingMaterial material;

    public float _Weight;
    public float weight { get { return _Weight; } }
    public float _Armor;
    public float armor { get { return _Armor; } }
    public EquipSlot _Slot;
    public EquipSlot slot { get { return _Slot; } }

    public CreatureStats _StatsAdded = new CreatureStats();
    public CreatureStats statsAdded { get { return _StatsAdded; } }

    void CraftUpdate()
    {
        itemName = material.itemName + " " + blueprint.itemName;
        _Weight = material.weight * blueprint.weightMod;
        _Armor = material.hardness * blueprint.armorMod;
        _Slot = blueprint.equipSlot;

        icon = blueprint.icon;

        _StatsAdded._Armor = armor;
        _StatsAdded._Weight = weight;
    }

    public override void UseItem(GameObject a_target)
    {
        CraftUpdate();
        if (a_target.GetComponent<Creature>())
        {
            if (a_target.GetComponent<Creature>().EquipArmor(this))
            {
                
                Debug.Log("Equiped Armor Item");
            }
            
        }
        else
        {
            Debug.LogError("Target to use EquipmentItem is not a Creature!");
        }
    }

    public override string WriteTooltip()
    {
        return base.WriteTooltip() + "\n" +
            armor + " Armor\n" +
            weight + " Weight";
    }

    public EquipmentItem(EquipmentBlueprint a_blue, CraftingMaterial a_craft)
    {
        blueprint = a_blue;
        material = a_craft;
        CraftUpdate();

        itemType = ItemType.Equipable;
        stackSize = 1;
        useable = true;
        consumable = false;
    }
}
