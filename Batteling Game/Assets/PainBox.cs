using UnityEngine;
using System.Collections;

public class PainBox : MonoBehaviour {

    //private Transform thisTrans;

    public BattlerCollider hitBox;

    public Battler.Alliance alliance = Battler.Alliance.PLAYER;
    public AttackEffects effects = new AttackEffects();

    public float linger;
    public int maxHits;
    public float hitDelay;
    public int facing;

    private int timesHit = 0;
    private float delayTime = 0;
    private Battler[] objectsToHit;

	// Use this for initialization
	void Start () {
        //thisTrans = gameObject.GetComponent<Transform>();

        objectsToHit = FindObjectsOfType<Battler>();

        //Debug.Log("Pain Box born.");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // Life span
        linger--;
        // delay between multiple hits
        if (delayTime > 0)
        {
            delayTime--;
        }
        // if hit max times or life span ends
	    if (linger <= 0 || timesHit >= maxHits)
        {
            GameObject.Destroy(this.gameObject);
        }
        // hits Battlers
        if (delayTime <= 0)
        {
            foreach (Battler i in objectsToHit)
            {
                if (hitBox.HitTest(i.hurtBox) && i.alliance != alliance)
                {
                    //Debug.Log("Pain Box hit " + i.name + "!");
                    i.GetHit(effects.push * facing, effects.lift, effects.damage);
                    //i.GetHit(0, 0, 5);
                    timesHit++;
                    //resets delay
                    delayTime += hitDelay;
                }
            }
        }
	}

    /*void updateHitBox()
    {
        hitBox.x1 = thisTrans.position.x - (width / 2);
        hitBox.x2 = thisTrans.position.x + (width / 2);
        hitBox.y1 = thisTrans.position.y - (height / 2);
        hitBox.y2 = thisTrans.position.y + (height / 2);
        hitBox.z = 0;
    }*/
    
    public void PhysicalAttackHitBox(BattlerPosition battler_position, BattlerBody battler_body, AttackEffects attack_effects, int battler_facing, float attack_range)
    {
        //thisTrans.position = new Vector3(battler_position.x, battler_position.y, 0);
        effects = attack_effects;

        facing = battler_facing;
        linger = 3;
        maxHits = 1;
        hitDelay = 3;

        if (battler_facing == 1)
        {
            hitBox.x1 = battler_position.x + (battler_body.width / 2);
            hitBox.x2 = battler_position.x + ((battler_body.width / 2) + attack_range);
        }
        else if (battler_facing == -1)
        {
            hitBox.x1 = battler_position.x - ((battler_body.width / 2) + attack_range);
            hitBox.x2 = battler_position.x - (battler_body.width / 2);
        }
        hitBox.y1 = battler_position.y;
        hitBox.y2 = battler_position.y + battler_body.height;
        hitBox.z = battler_position.lane;
    }
}
