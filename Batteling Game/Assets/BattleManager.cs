using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour {

    public static BattleManager Manager;

    public GameObject DamageTracePrefab;

    /*[SerializeField]
    private int players = 1;
    [SerializeField]
    private int monsters = 1;*/
    [SerializeField]
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

    private void AddPlayers()
    {

    }
    //public

    public void TraceDamage(Vector3 a_position, float a_display)
    {
        DamageTracePrefab.transform.position = a_position + new Vector3(0, 0, -0.1f);
        DamageTracePrefab.GetComponent<DamageText>().display = a_display.ToString();
        GameObject.Instantiate(DamageTracePrefab);
    }
    public void TraceTotal(Vector3 a_position, float a_damage)
    {
        DamageTracePrefab.transform.position = a_position + new Vector3(0, 0, -0.1f);
        DamageTracePrefab.GetComponent<DamageText>().display = "Total " + a_damage.ToString();
        GameObject.Instantiate(DamageTracePrefab);
    }
}
