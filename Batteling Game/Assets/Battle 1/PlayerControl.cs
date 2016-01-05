using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    private Creature thisControling;
    //private Transform thisPos;
    private Rigidbody2D thisBody;

    private Camera theCamera;

	// Use this for initialization
	void Start () {
	    thisControling = this.GetComponent<Creature>();
        //thisPos = this.GetComponent<Transform>();
        thisBody = this.GetComponent<Rigidbody2D>();
        //Debug.Log(this.gameObject.name);
        theCamera = FindObjectOfType<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //Look at mouse
        Vector2 mousePos = Input.mousePosition;
        //mousePos.z = 10.0f; //The distance from the camera to the player object
        Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
        lookPos = lookPos - transform.position;
        //float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        
        //Move with mouse
        if (Input.GetAxis("Fire2") > 0)
        {
            thisBody.AddForce(lookPos.normalized * ((this.thisControling.stats.moveSpeed * thisControling.mobility) * 10));
        }
        else
        {
            //Move with directional buttons
            thisBody.AddForce(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * ((this.thisControling.stats.moveSpeed * thisControling.mobility) * 10));
        }
        
        //Camera follow
        //Vector3 playerScreenSpace = Camera.main.WorldToScreenPoint(this.thisControling.transform.position);
        //Vector3 cameraScreenPoint = playerScreenSpace + (Input.mousePosition - playerScreenSpace) / 8;
        //Vector3 cameraWorldPoint = Camera.main.ScreenToWorldPoint(cameraScreenPoint);
        //theCamera.transform.position = new Vector3(cameraWorldPoint.x, cameraWorldPoint.y, -10);

        theCamera.transform.position = new Vector3(this.thisControling.transform.position.x, this.thisControling.transform.position.y, -10);

        //Targeting/Interacting

        Creature[] creatures = FindObjectsOfType<Creature>();
        bool clickedSpace = true;
        foreach (Creature i in creatures)
        {
            if (Input.GetButton("Fire1"))
            {
                /*if (i == thisControling)
                {
                    continue;
                }*/
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 iPos = new Vector2(i.transform.position.x, i.transform.position.y);
                if (Mathf.Abs((mousePos - iPos).magnitude) < i.stats.size)
                {
                    //Debug.Log("Targeting: " + i.name);
                    thisControling.target = i;
                    thisControling.BasicAttack();
                    clickedSpace = false;
                }

                if (clickedSpace && thisControling.attackTime == 0)
                {
                    thisControling.target = null;
                }
            }
        }
        
	}
}
