using UnityEngine;
using System.Collections;

[System.Serializable]
public class BattlerCollider
{
    // Simple AABB
    public float x1 = 0, x2 = 0;
    public float y1 = 0, y2 = 0;
    public int z = 0;

    public bool HitTest(BattlerCollider other)
    {
        //ErrorCheck(this);
        //ErrorCheck(other);
        return (x1 <= other.x2 && x2 >= other.x1) && (y1 <= other.y2 && y2 >= other.y1) && z == other.z;
    }

    void ErrorCheck(BattlerCollider check)
    {
        if (check.x1 > check.x2)
        {
            Debug.LogError("BattlerCollider has negative boundries: x1 > x2!");
        }
        if (check.y1 > check.y2)
        {
            Debug.LogError("BattlerCollider has negative boundries: y1 > y2!");
        }
    }
}
[System.Serializable]
public class BattlerPosition
{
    public float x = 0;
    public float y = 0;
    public int lane = 0;

    public int z
    {
        get
        {
            return lane;
        }
    }
    public Vector2 XY
    {
        get
        {
            return new Vector2(x, y);
        }
        set
        {
            x = value.x;
            y = value.y;
        }
    }
    public Vector3 XYZ
    {
        get
        {
            return new Vector3(x, y, (float)lane);
        }
    }
}
[System.Serializable]
public class BattlerBody
{
    public float width = 1;
    public float height = 1;
    public float density = 1;

    public float weight
    {
        get
        {
            float ret = 0;
            ret += (width + height) / 2;
            ret *= density;
            return ret;
        }
    }
    public float scale
    {
        get
        {
            float ret = 0;
            ret += (width + height) / 2;
            return ret;
        }
    }
}


public class CharacterStats
{
    public int level = 1;

    private int StrengthLevel = 0;
    public int strength
    {
        get
        {
            return level + StrengthLevel + 4;
        }
    }
    private int MagicLevel = 0;
    public int magic
    {
        get
        {
            return level + MagicLevel + 4;
        }
    }
    private int AgilityLevel = 0;
    public int agility
    {
        get
        {
            return level + AgilityLevel + 4;
        }
    }
}
public class CharacterSkills
{
    public AttackParamaters materialNeutral = AttackParamaters.BasicAttack;
    public AttackParamaters materialUp = AttackParamaters.BasicAttack;
    public AttackParamaters materialSide = AttackParamaters.BasicAttack;
    public AttackParamaters materialDown = AttackParamaters.BasicAttack;
}


//-------------------------------------------------------------------------------------------------------------------------------
public class Battler : MonoBehaviour {
    //-----------------------------------------------------------BATTLER---------------------------------------------------------
    public enum Action { NEUTRAL = 0, ACTING, STAGGERED }
    public enum Alliance { UNALIENED = 0, PLAYER, ENEMY }
    
    private SpriteRenderer thisRenderer;
    private Animator thisAnimation;
    private Transform thisTransform;
    // Battle Variables
    public Alliance alliance = Alliance.PLAYER;
    public float healthPoints = 100;
    public CharacterSkills skills = new CharacterSkills();
    // Movement variables
    public BattlerPosition position;
    public Vector2 velocity = new Vector2(0, 0);
    public float moveSpeed = 0.1f;
    public bool airborne
    {
        get
        {
            if (position.y > 0.01f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    // Control Variables
    [Range(-1, 1)]
    public int movementX;
    [Range(-1, 1)]
    public int movementY;
    private int laneChange;
    //public bool jump = false;
    public bool attackBasic = false;
    public bool matSkill = false;
    // Action variables
    private Action currentState = Action.NEUTRAL;
    private float action;
    private float execution;
    private bool canAct = true;
    private ActionFrameData currentActionFrames;
    private AttackParamaters currentAttack;
    private BattlerCollider attachedHitBox = new BattlerCollider();
    // Collision Variables
    public BattlerBody body;
    public BattlerCollider hurtBox
    {
        get
        {
            BattlerCollider ret = new BattlerCollider();
            ret.y1 = position.y;
            ret.y2 = position.y + body.height;

            ret.x1 = position.x - (body.width / 2);
            ret.x2 = position.x + (body.width / 2);

            ret.z = position.z;

            return ret;
        }
    }
    public Vector2 center
    {
        get
        {
            return new Vector2(position.x, position.y + (body.height / 2));
        }
    }
    public float rightSide
    {
        get
        {
            return position.x + (body.width / 2);
        }
    }
    public float leftSide
    {
        get
        {
            return position.x - (body.width / 2);
        }
    }
    public int facing
    {
        get
        {
            if (thisRenderer.flipX)
            {
                return -1;
            } else
            {
                return 1;
            }
        }
    }
    // Extra Stuff
    //public float hitEffect = 0;
    //-----------------------------------------------------------START-----------------------------------------------------------
    void Start ()
    {
        // Renderer
        thisRenderer = GetComponent<SpriteRenderer>();
        if (!thisRenderer)
        {
            Debug.LogError("Battler " + this.name + " missing SprteRenderer Component!");
        }
        // Animation
        thisAnimation = GetComponent<Animator>();
        if (!thisAnimation)
        {
            Debug.LogError("Battler " + this.name + " missing Animation Component!");
        }
        // Position Starting
        thisTransform = GetComponent<Transform>();
        thisTransform.position = position.XYZ;

        // TEMPORARY-TEMPORARY-TEMPORARY-TEMPORARY-TEMPORARY-TEMPORARY-TEMPORARY-TEMPORARY-TEMPORARY-
        AttackParamaters testAttack1 = new AttackParamaters();
        AttackEffects testEffects1 = new AttackEffects();
        testEffects1.push = 0.1f;
        testEffects1.lift = 0;
        testEffects1.damage = 10;
        ActionFrameData testFrameData1 = new ActionFrameData();
        testFrameData1.execution = 30;
        testFrameData1.combo = 15;
        testFrameData1.lag = 15;
        testFrameData1.strikeFrames = new int[2] { 15, 25 };

        testAttack1.frameData = testFrameData1;
        testAttack1.effects = testEffects1;
        testAttack1.animationName = "thrust";

        skills.materialDown = testAttack1;
        skills.materialSide = testAttack1;
        // UP
        AttackParamaters testAttack2 = new AttackParamaters();
        AttackEffects testEffects2 = new AttackEffects();
        testEffects2.push = 0;
        testEffects2.lift = 0.1f;
        testEffects2.damage = 10;
        ActionFrameData testFrameData2 = new ActionFrameData();
        testFrameData2.execution = 10;
        testFrameData2.combo = 45;
        testFrameData2.lag = 1;
        testFrameData2.strikeFrames = new int[1] { 5 };

        testAttack2.frameData = testFrameData2;
        testAttack2.effects = testEffects2;
        //testAttack2.animationName = "attackBasic";

        skills.materialUp = testAttack2;
        // END TEMPORARY
    }
    //-----------------------------------------------------------UPDATE----------------------------------------------------------
    void Update()
    {
        /*if (hitEffect > 0)
        {
            thisRenderer.color = Color.Lerp(Color.white, Color.red * 2, hitEffect);
            hitEffect -= Time.deltaTime * 8;
        }*/
    }
    //===========================================================FIXEDUPDATE=====================================================
	void FixedUpdate () {
        // Uncontrolled: ----------------------------------------UNCONTROLLED----------------------------------------------------
        // Staggered 
        if (currentState == Action.STAGGERED)
        {
            canAct = false;
        }
        // Action Time
        if (action > 0)
        {
            if (currentState != Action.STAGGERED)
            {
                action--;
            }
            else
            {// If Staggered and Airborne, can't recover from Staggered
                if (!airborne)
                {
                    action--;
                }
            }
        }
        if (action <= 0)  
        {// If action timer is 0, state is Neutral and canAct
            action = 0;
            currentState = Action.NEUTRAL;
            canAct = true;
            // Neutral Animations
            if (movementX == 0 && movementY == 0)
            {
                thisAnimation.Play("idle");
            } else
            {
                thisAnimation.Play("move");
            }
        }
        // Ground Friction
        if (airborne == false)
        {
            velocity.x *= 0.9f;

            if (velocity.x < 0.01f && velocity.x > -0.01f)
            {
                velocity.x = 0;
            }
        }
        // Falling
        if (airborne)
        {
            velocity.y -= body.weight * 0.01f;
        }

        // Controlled: ------------------------------------------CONTROLLED------------------------------------------------------
        if (currentState == Action.NEUTRAL)
        {
            // Moving
            if (movementX != 0)
            {
                if (movementX >= 1)
                {
                    position.x += moveSpeed;
                    thisRenderer.flipX = false;
                }
                else if (movementX <= -1)
                {
                    position.x -= moveSpeed;
                    thisRenderer.flipX = true;
                }
            }
            // Lane Change
            if (movementY != 0)
            {
                if (movementY >= 1 && position.lane < BattleManager.Manager.field.lanes - 1)
                {
                    laneChange++;
                }
                else if (movementY <= -1 && position.lane > 0)
                {
                    laneChange--;
                }
            }
            else
            {
                laneChange = 0;
            }
            if (laneChange >= 30)
            {
                position.lane += 1;
                laneChange = 0;
            }
            if (laneChange <= -30)
            {
                position.lane -= 1;
                laneChange = 0;
            }
            // Jumping
            /*if (jump && airborne == false)
            {
                velocity.y = jumpStrength;
                jump = false;
            }*/
        }
        // Using a Basic Attack ---------------------------------ATTACKING-------------------------------------------------------
        if (attackBasic && canAct)
        {
            Debug.Log("Basic attack Input!");
            ExecuteAction(AttackParamaters.BasicAttack);
            
            attackBasic = false;
        }
        // Using a Mat Skill ------------------------
        if (matSkill && canAct)
        {
            // Side attack
            if (movementX != 0)
            {
                Debug.Log("Side attack Input!");
                ExecuteAction(skills.materialSide);
            }
            else if (movementY != 0)
            {
                // UP Attack
                if (movementY > 0)
                {
                    Debug.Log("Up attack Input!");
                    ExecuteAction(skills.materialUp);
                }//Down Attack
                else if (movementY < 0)
                {
                    Debug.Log("Down attack Input!");
                    ExecuteAction(skills.materialDown);
                }
            }// Neutral Attack
            else
            {
                Debug.Log("Neutral attack Input!");
                ExecuteAction(skills.materialNeutral);
            }

            matSkill = false;
        }

        // While Attacking(Acting) ==============================WHILEACTING=====================================================
        if (currentState == Action.ACTING)
        {
            //make hits on strike frames
            foreach (int i in currentActionFrames.strikeFrames)
            {
                if (execution == i)
                {
                    SetAttHitBox(1.0f);
                    // Hit other battlers
                    foreach (Battler other in BattleManager.Manager.allBattlers)
                    {
                        if (other != this && other.alliance != alliance)
                        {
                            if (attachedHitBox.HitTest(other.hurtBox))
                            {
                                other.GetHit(currentAttack.effects.push * facing, currentAttack.effects.lift, currentAttack.effects.damage);
                            }
                        }
                    }
                }
            }
            // canAct in combo frames
            if (execution > currentActionFrames.execution && execution <= currentActionFrames.comboLimit)
            {
                canAct = true;
            }
            else
            {
                canAct = false;
            }
            execution++;
        }
        // UnControlled2 ----------------------------------------UNCONTROLLED2---------------------------------------------------
        // Push other Battlers
        //otherBattlers = FindObjectsOfType<Battler>();
        foreach (Battler other in BattleManager.Manager.allBattlers)
        {
            if (other != this)
            {
                if (hurtBox.HitTest(other.hurtBox))
                {
                    float xDif = position.x - other.position.x;//from other
                    float compWidth = (body.width / 2) + (other.body.width / 2);
                    //float weightDif = body.weight - other.body.weight;

                    if (xDif < 0)
                    {
                        position.x -= ((compWidth - Mathf.Abs(xDif)) / 4) * (other.body.weight / body.weight);
                    }
                    else
                    {
                        position.x += ((compWidth - Mathf.Abs(xDif)) / 4) * (other.body.weight / body.weight);
                    }
                    
                    //Debug.Log("Battlers Touching");
                }
            }
        }
        // Touch Ground
        if (position.y < 0)
        {
            position.y = 0.0f;
            velocity.y = 0;
        }
        // Boundries
        if (position.x < BattleManager.Manager.field.leftBarrier)
        {
            position.x = BattleManager.Manager.field.leftBarrier;
            velocity.x = 0;
        }
        if (position.x > BattleManager.Manager.field.rightBarrier)
        {
            position.x = BattleManager.Manager.field.rightBarrier;
            velocity.x = 0;
        }
        // Position Updating
        position.XY += velocity;
        thisTransform.position = position.XYZ;


        // Test Stuff
    }
//---------------------------------------------------------------GETHIT----------------------------------------------------------
    public void GetHit(float xPush, float yPush, float damage)
    {
        healthPoints -= damage;
        action = 30;
        execution = 0;
        currentState = Action.STAGGERED;
        velocity = new Vector2(xPush, yPush) / body.weight;
        thisAnimation.Play("hurt", -1, 0);
        
        //hitEffect = 1;
    }
    void ExecuteAction(AttackParamaters p_attack)
    {
        currentAttack = p_attack;
        currentActionFrames = p_attack.frameData;
        action = p_attack.frameData.fullFrames;

        if (thisAnimation)
        {
            if (currentAttack.animationName != null)
            {
                thisAnimation.Play(currentAttack.animationName, -1, 0);
            }
            else
            {
                thisAnimation.Play("attackBasic", -1, 0);
            }
        }
        currentState = Action.ACTING;
        execution = 0;
    }

    void SetAttHitBox(float a_range)
    {
        attachedHitBox.y1 = position.y;
        attachedHitBox.y2 = position.y + body.height;
        attachedHitBox.z = position.z;

        if (facing > 0)
        {
            attachedHitBox.x1 = rightSide;
            attachedHitBox.x2 = rightSide + a_range;
        }
        else if ( facing < 0)
        {
            attachedHitBox.x1 = leftSide - a_range;
            attachedHitBox.x2 = leftSide;
        }
    }
}
