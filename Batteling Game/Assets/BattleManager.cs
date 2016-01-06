using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour {

    public static BattleManager Manager;

    private Battler[] AllBattlers;
    public Battler[] allBattlers
    {
        get
        {
            return AllBattlers;
        }
    }
    private BattleField Field = new BattleField();
    public BattleField field
    {
        get
        {
            return Field;
        }
    }

	// Use this for initialization
	void Start () {
        // Make Singleton Manager
	    if (Manager == null)
        {
            DontDestroyOnLoad(gameObject);
            Manager = this;
        }
        else if (Manager != this)
        {
            Destroy(gameObject);
        }

        AllBattlers = GameObject.FindObjectsOfType<Battler>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
