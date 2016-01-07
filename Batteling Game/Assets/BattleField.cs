using UnityEngine;
using System.Collections;

[System.Serializable]
public class BattleField {

    [Range(1,5)]
    public int lanes = 3;
    public float width = 20;

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


}
