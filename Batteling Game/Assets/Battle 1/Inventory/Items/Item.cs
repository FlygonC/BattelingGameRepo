using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item : ScriptableObject {

    public enum ItemType
    {
        Treasure,
        Equipable,
        Weapon,
        Crafting,
        Blueprint,
        Food,
    }

    //public int itemID;
    public string itemName;
    public ItemType itemType;
    public int stackSize = 1;
    public bool useable;
    public bool consumable;
    //public int count;
    //public int iconIndex;
    public Sprite icon;

    public virtual void UseItem(GameObject a_target)
    {
        Debug.LogWarning("UseItem called from Item class; Item does not have a use.");
    }

    public virtual string WriteTooltip()
    {
        string returnString;
        returnString = itemName + "\n" + itemType.ToString();
        if (consumable)
        {
            returnString += " Consumable";
        }
        return returnString;
    }

    public Item(int a_id, string a_name, ItemType a_type, int a_stack, int a_iconNumber)
    {
        //itemID = a_id;
        itemName = a_name;
        itemType = a_type;
        stackSize = a_stack;
        //count = 1;
        //iconIndex = a_iconNumber;
        icon = Resources.LoadAll<Sprite>("Icons34")[a_iconNumber];
    }

    public Item()
    {

    }
}
