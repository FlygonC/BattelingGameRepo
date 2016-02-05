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
            return new Vector3(X, Y, Z);
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
    
    private float tallest = 0;
    //[SerializeField]
    //private float shortest = 0;

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
    [Range(0.001f, 1)]
    public float closness = 0.8f;
    [Range(0, 5)]
    public float YMin = 2;
    [Range(1, 2)]
    public float YZoomFactor = 1.5f;

    void Start () {
	    
	}
	
	void Update () {
        // Camera
        X = 0;
        Y = 0;
        Z = 0;
        furthestLeft = Mathf.Infinity;
        furthestRight = -Mathf.Infinity;
        tallest = -Mathf.Infinity;
        //shortest = 0;
        foreach (Battler i in BattleManager.Manager.allBattlers)
        {
            if (i.alive)
            {
                //X += i.position.x;
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
                
                if (i.body.height > tallest)
                {
                    tallest = i.body.height;
                }
            }
        }
        X = furthestLeft + (furthestDif / 2);
        //Y = Y / BattleManager.Manager.allBattlers.Length;
        Y = Mathf.Max(YMin, tallest);
        Z = (-(Mathf.Max(zoomMinimum, furthestDif)) / (aspect * closness)) - (Y / YZoomFactor);

        // Set Position
        this.transform.position += currentFinalDif * persistence;
	}
}
