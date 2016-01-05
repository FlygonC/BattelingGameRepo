using UnityEngine;
using System.Collections;

public enum EquipSlot { Chest, Head, Arm, Leg, Belt, }

[System.Serializable]
public class EquipmentBlueprint : Item {

    public EquipSlot equipSlot;
    public float weightMod;
    public float armorMod;

    public EquipmentBlueprint()
    {
        itemType = ItemType.Blueprint;
        stackSize = 1;
        useable = false;
        consumable = false;
    }
}
