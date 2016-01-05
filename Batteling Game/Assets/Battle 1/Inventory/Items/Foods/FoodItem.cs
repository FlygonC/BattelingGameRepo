using UnityEngine;
using System.Collections;

[System.Serializable]
public class FoodItem : Item {

    public override void UseItem(GameObject a_target)
    {
        //base.UseItem(a_target);
        Debug.Log("Consumed FoodItem!");

    }

    public FoodItem()
    {
        itemType = ItemType.Food;
        stackSize = 5;
        useable = true;
        consumable = true;
    }
}
