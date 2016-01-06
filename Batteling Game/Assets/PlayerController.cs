using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [Tooltip("Drag a Battler from the hierarchy here to change control.")]
    public GameObject inControl;

	// Use this for initialization
	void Start () {
        ChangeControl();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetAxis("Horizontal") > 0)
        {
            inControl.GetComponent<Battler>().movementX = 1;
        } 
        if (Input.GetAxis("Horizontal") < 0)
        {
            inControl.GetComponent<Battler>().movementX = -1;
        }
        if (Input.GetAxis("Horizontal") == 0)
        {
            inControl.GetComponent<Battler>().movementX = 0;
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            inControl.GetComponent<Battler>().movementY = 1;
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            inControl.GetComponent<Battler>().movementY = -1;
        }
        if (Input.GetAxis("Vertical") == 0)
        {
            inControl.GetComponent<Battler>().movementY = 0;
        }

        //inControl.GetComponent<Battler>().jump = Input.GetButton("Fire1");
        inControl.GetComponent<Battler>().attackBasic = Input.GetButton("Fire1");
    }

    void ChangeControl()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Battler>())
        {
            inControl = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
