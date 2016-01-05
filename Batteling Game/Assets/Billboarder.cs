using UnityEngine;
using System.Collections;

public class Billboarder : MonoBehaviour {

    private Camera cam;

    // Use this for initialization
    void Start () {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        this.transform.rotation = cam.transform.rotation;
	}
}
