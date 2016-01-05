using UnityEngine;
using System.Collections;

public class BattleField : MonoBehaviour {

    [Range(1,5)]
    public int lanes = 1;
    public float width = 10;

    public float leftBarrier
    {
        get
        {
            return -width / 2;
        }
    }
    public float rightBarrier
    {
        get
        {
            return width / 2;
        }
    }

	// Use this for initialization
	void Start () {
        this.gameObject.tag = "BattleField";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
