using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [Tooltip("Drag a Battler from the hierarchy here to change control.")]
    public Battler inControl;

	// Use this for initialization
	void Start () {
        //ChangeControl();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetAxis("Horizontal") > 0)
        {
            inControl.movementX = 1;
        } 
        if (Input.GetAxis("Horizontal") < 0)
        {
            inControl.movementX = -1;
        }
        if (Input.GetAxis("Horizontal") == 0)
        {
            inControl.movementX = 0;
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            inControl.movementY = 1;
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            inControl.movementY = -1;
        }
        if (Input.GetAxis("Vertical") == 0)
        {
            inControl.movementY = 0;
        }
        
        inControl.attackBasic = Input.GetButton("Fire1");
        inControl.matSkill = Input.GetButton("Fire2");
        inControl.defend = Input.GetButton("Fire3");


        this.transform.position = new Vector3(inControl.position.x, inControl.position.y + inControl.body.height + 0.15f, inControl.position.z);
    }

    void ChangeControl()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Battler>())
        {
            inControl = GameObject.FindGameObjectWithTag("Player").GetComponent<Battler>();
        }
    }
}
