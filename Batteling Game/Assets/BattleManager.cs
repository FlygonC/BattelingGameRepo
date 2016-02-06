using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {

    public static BattleManager Manager;

    public GameObject DamageTracePrefab;

    [SerializeField]
    private int numberOfBattlers = 0;
    /*[SerializeField]
    private int players = 0;
    [SerializeField]
    private int monsters = 0;*/
    [SerializeField]
    public List<Battler> AllBattlers = new List<Battler>();

    [SerializeField]
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


        SpawnBattler(1 - 1, new BattlerPosition(-1, 0, 1), Battler.Alliance.PLAYER);
        SpawnBattler(1 - 1, new BattlerPosition( 1, 0, 1), Battler.Alliance.ENEMY);
        SpawnBattler(1 - 1, new BattlerPosition( 2, 0, 1), Battler.Alliance.ENEMY);


        //AllBattlers = GameObject.FindObjectsOfType<Battler>();


        //FindObjectOfType<PlayerController>().GetComponent<PlayerController>().inControl = AllBattlers[0];
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    private void SpawnBattler(int _prefabIndex, BattlerPosition _pos, Battler.Alliance _alliance)
    {
        GameObject spawn = MasterDatabase.MasterData.battlerPrefabs[_prefabIndex];
        
        GameObject.Instantiate(spawn);
        //spawn.GetComponent<Battler>().position = _pos;
        //spawn.GetComponent<Battler>().alliance = _alliance;
        //spawn.name = "Battler " + numberOfBattlers.ToString();
        numberOfBattlers++;
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
        DamageTracePrefab.GetComponent<DamageText>().display = "Total\n" + a_damage.ToString();
        GameObject.Instantiate(DamageTracePrefab);
    }
}
