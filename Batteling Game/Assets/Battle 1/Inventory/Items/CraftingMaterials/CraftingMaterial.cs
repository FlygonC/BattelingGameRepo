using UnityEngine;
using System.Collections;

[System.Serializable]
public class CraftingMaterial : Item {
    //public string displayName;
    public float weight;
    public float sharpness;
    public float hardness;

    public override string WriteTooltip()
    {
        return base.WriteTooltip() + "\n" +
            sharpness + " Sharpness\n" +
            hardness + " Hardness\n" +
            weight + " Weight";
    }

    public CraftingMaterial()
    {
        itemType = ItemType.Crafting;
        stackSize = 1;
        useable = false;
        consumable = false;
    }
}
