using UnityEngine;
using System.Collections;

public enum ItemType { CraftedWeapon, CraftingMaterial, Treasure};

public class DroppedItem : MonoBehaviour {

    public ItemType itemType;

    public GameObject _Item;

    private SpriteRenderer thisSprite;

	// Use this for initialization
	void Start () {
        thisSprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        thisSprite.sprite = _Item.GetComponent<iItem>().icon;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name + " touched Dropped Item.");
        if (other.gameObject.GetComponent<Creature>() == true) {
            _Item.GetComponent<iItem>().Use(other.gameObject.GetComponent<Creature>());
        }
    }
}
