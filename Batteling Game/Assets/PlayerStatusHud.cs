using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStatusHud : MonoBehaviour {

    public Battler monitorTarget;

    private Image greenBar;
	// Use this for initialization
	void Start () {
        greenBar = GameObject.Find("HPBar").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        greenBar.fillAmount = (monitorTarget.hitPoints / monitorTarget.character.hpMax) * 0.75f + 0.25f;
	}
}
