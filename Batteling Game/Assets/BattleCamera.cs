using UnityEngine;
using System.Collections;

public class BattleCamera : MonoBehaviour {

    private float X = 0;
    private float Y = 0;
    private float Z = 0;
    public Vector3 finalPosition
    {
        get
        {
            return new Vector3(X, Y - (Z / 10), Z);
        }
    }
    private Vector3 currentFinalDif
    {
        get
        {
            return finalPosition - this.transform.position;
        }
    }

    private float furthestLeft = 0;
    private float furthestRight = 0;
    private float furthestDif
    {
        get
        {
            return furthestRight - furthestLeft;
        }
    }

    private float aspect
    {
        get
        {
            return (float)Screen.width / Screen.height;
        }
    }

    public float zoomMinimum = -3.5f;
    [Range(0,1)]
    public float persistence = 0.15f;
    [Range(0, 1)]
    public float closness = 0.8f;

    void Start () {
	    
	}
	
	void Update () {
        // Camera
        X = 0;
        Y = 0;
        Z = 0;
        furthestLeft = Mathf.Infinity;
        furthestRight = -Mathf.Infinity;
        foreach (Battler i in BattleManager.Manager.allBattlers)
        {
            X += i.position.x;
            Y += i.body.height / 2;
            //Z += i.position.z;
            if (i.position.x < furthestLeft)
            {
                furthestLeft = i.position.x;
            }
            if (i.position.x > furthestRight)
            {
                furthestRight = i.position.x;
            }
        }
        X = furthestLeft + (furthestDif / 2);
        Y = (Y / BattleManager.Manager.allBattlers.Length);
        Z = -(Mathf.Max(zoomMinimum, furthestDif)) / (aspect * closness);

        // Set Position
        this.transform.position += currentFinalDif * persistence;
	}
}
