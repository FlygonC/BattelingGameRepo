using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {

    //public List<Item> itemData = new List<Item>();

    public List<CraftingMaterial> craftingMaterials = new List<CraftingMaterial>();
    public List<WeaponBlueprint> weaponBlueprints = new List<WeaponBlueprint>();
    public List<EquipmentBlueprint> equipBlueprints = new List<EquipmentBlueprint>();

    public List<FoodItem> foodItems = new List<FoodItem>();

	// Use this for initialization
	void Start () {

        /*for (int i = 0; i < craftingMaterials.Count; i++)
        {
            craftingMaterials[i].itemID = i;
        }
        for (int i = 0; i < blueprints.Count; i++)
        {
            blueprints[i].itemID = i;
        }*/
	}

    public CraftingMaterial GetCraftingMaterial(int a_id)
    {
        return craftingMaterials[a_id];
    }
    public WeaponBlueprint GetWeaponBlueprint(int a_id)
    {
        return weaponBlueprints[a_id];
    }
    public EquipmentBlueprint GetEquipmentBlueprint(int a_id)
    {
        return equipBlueprints[a_id];
    }
    public FoodItem GetFoddItem(int a_id)
    {
        return foodItems[a_id];
    }
}
