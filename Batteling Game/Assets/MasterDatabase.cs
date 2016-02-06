using UnityEngine;
using System.Collections;

public class MasterDatabase : MonoBehaviour {

    public static MasterDatabase MasterData;

    public GameObject[] battlerPrefabs = new GameObject[0];


	// Use this for initialization
	void Start () {
        // Make Singleton Data
        if (MasterData == null)
        {
            DontDestroyOnLoad(gameObject);
            MasterData = this;
        }
        else if (MasterData != this)
        {
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
