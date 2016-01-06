using UnityEngine;
using System.Collections;

public class BattleField {

    [Range(1,5)]
    public int lanes = 3;
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


}
