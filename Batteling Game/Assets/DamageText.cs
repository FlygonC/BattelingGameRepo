using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

    private Text text;
    public string display;

    public float panSpeed;
    public float lifeTime;

	// Use this for initialization
	void Start () {
        text = GetComponentInChildren<Text>();
        text.text = display;
        panSpeed = 8;
        lifeTime = 1.5f;
        //transform.position -= new Vector3(0.16f, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        //text = GetComponentInChildren<Text>();
        if (lifeTime <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
        if (panSpeed > 0)
        {
            panSpeed *= 0.8f;
        }
        this.transform.position += new Vector3(0, panSpeed * 0.01f, 0);
        lifeTime -= Time.deltaTime;
    }
}
