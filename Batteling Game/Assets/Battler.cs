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

    public BattlerCollider SetBoxByCenter(Vector2 a_center, int a_z, float a_width, float a_height)
    {
        x1 = a_center.x - (a_width / 2);
        x2 = a_center.x + (a_width / 2);
        y1 = a_center.y - (a_height / 2);
        y2 = a_center.y + (a_height / 2);
        z = a_z;
        return this;
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
    //public float density = 1;
    public float weight = 1;

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


public struct DamageCVars
{
    //type
    public float amount;
    public float attackerAcc;
}


//-------------------------------------------------------------------------------------------------------------------------------
[DisallowMultipleComponent]
public class Battler : MonoBehaviour {
    //-----------------------------------------------------------BATTLER---------------------------------------------------------
    public enum Action { NEUTRAL = 0, ACTING, STAGGERED }
    public enum Alliance { UNALINED = -1, PLAYER, ENEMY }
    
    private SpriteRenderer thisRenderer;
    private Animator thisAnimation;
    private Transform thisTransform;

    // Battle Variables
    public Alliance alliance = Alliance.PLAYER;
    public Character character = new Character();
    public CharacterSkills skills
    {
        get
        {
            return character.skills;
        }
    }
    public float hitPoints
    {
        get
        {
            return character.hp;
        }
        set
        {
            character.hp = Mathf.Clamp(value, 0, character.hpMax);
        }
    }

    public Stats statsModifier = new Stats();

    public float attackDamage
    {
        get
        {
            return Mathf.Round(character.attackDamage * statsModifier.attackDamage);
        }
    }
    public float magicDamage
    {
        get
        {
            return Mathf.Round(character.magicDamage * statsModifier.magicDamage);
        }
    }
    public float armor
    {
        get
        {
            return character.armor;
        }
    }
    public float accuracy
    {
        get
        {
            return Mathf.Round(character.accuracy * statsModifier.accuracy);
        }
    }
    public float evade
    {
        get
        {
            return Mathf.Round(character.evasion * statsModifier.evasion);
        }
    }


    public float maxEndurance
    {
        get { return Mathf.Round(character.hpMax * 0.1f); }
    }
    public float endurance = 0;

    //public float effectiveBlock = 0.75f;
    private bool stagImmune
    {
        get
        {
            return this.execution < currentActionFrames.execution && currentActionFrames.superArmor && currentState == Action.ACTING;
        }
    }

    // Movement variables
    public BattlerPosition position;
    private float softZ;
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
    public bool defend = false;
    // Action variables
    private Action currentState = Action.NEUTRAL;
    private float action;
    private float execution;
    private bool canAttack = true;
    private bool defending = false;
    private ActionFrameData currentActionFrames = new ActionFrameData();
    private MaterialSkillBasic currentAttack;
    private BattlerCollider attachedHitBox = new BattlerCollider();
    public float actionPoints = 3;
    public float actionPointsMax
    {
        get { return 3; }
    }
    private bool canAction
    {
        get
        {
            return canAttack && actionPoints >= 1.0f;
        }
    }
    // Collision Variables
    public BattlerBody body;
    public BattlerCollider hurtBox
    {
        get
        {
            BattlerCollider ret = new BattlerCollider();
            return ret.SetBoxByCenter(center, position.z, body.width, body.height);
        }
    }
    public Vector3 center
    {
        get
        {
            return new Vector3(position.x, position.y + (body.height / 2), position.z);
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
    public int facing = 1;
    // Extra Stuff
    private int comboHits = 0;
    private float comboDamage = 0;
    private float comboTime = 0;
    private bool combo = false;
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
        softZ = position.z;
        // TEMPORARY-TEMPORARY-TEMPORARY-TEMPORARY-TEMPORARY-TEMPORARY-TEMPORARY-TEMPORARY-TEMPORARY-
        character.skills = CharacterSkills.TestSkills;
        // END TEMPORARY
        hitPoints = character.hpMax;
        statsModifier.SetAll(1);
        endurance = maxEndurance;
    }
    //-----------------------------------------------------------UPDATE----------------------------------------------------------
    void Update()
    {
        if (facing != 0)
        {
            if (facing > 0)
            {
                thisTransform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else if (facing < 0)
            {
                thisTransform.rotation = new Quaternion(0, 180, 0, 0);
            }
        } else
        {
            facing = 1;
        }

        character.UpdateAllStats();
        // Combo Damage
        if (comboTime > 0)
        {
            comboTime -= Time.deltaTime;
        }
        if (comboHits >= 2 && comboDamage > 0)
        {
            combo = true;
        }
        if (combo == true && comboTime <= 0)
        {
            BattleManager.Manager.TraceTotal(center + new Vector3(0, body.height / 2, 0), comboDamage);
        }
        /*if (comboHits > 2)
        {
            comboHits = 0;
        }*/
        if (comboTime <= 0)
        {
            combo = false;
            comboHits = 0;
            comboDamage = 0;
        }
    }
    //===========================================================FIXEDUPDATE=====================================================
	void FixedUpdate () {
        // Uncontrolled: ----------------------------------------UNCONTROLLED----------------------------------------------------
        // Staggered 
        if (currentState == Action.STAGGERED)
        {
            canAttack = false;
            //thisRenderer.color = new Color(1, 1, 1, 0.25f);
        }
        // Endurance
        if (endurance < maxEndurance && currentState != Action.ACTING)
        {
            endurance += maxEndurance / (60);
            if (endurance >= maxEndurance)
            {
                endurance = maxEndurance;
            }
        }
        if (endurance < 0)
        {
            endurance = 0;
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
            canAttack = true;
            // Neutral Animations ###################################
            if (!airborne)
            {
                if (!defend)
                {// no airbore or defending
                    if (movementX == 0 && movementY == 0)
                    {
                        thisAnimation.Play("idle");
                    }
                    else
                    {
                        thisAnimation.Play("move");
                    }
                }
                else
                {
                    thisAnimation.Play("defend");
                }
            }
            else
            {
                thisAnimation.Play("jump");
            }
        }
        //Action Points
        if (currentState != Action.ACTING)
        {
            if (actionPoints < actionPointsMax)
            {
                if (!defending && movementX == 0 && movementY == 0)
                {
                    actionPoints += 1.0f / (30.0f);
                }
                /*else
                {
                    actionPoints += 0.25f / (60.0f);
                }*/
            }
            if (actionPoints >= actionPointsMax)
            {
                actionPoints = actionPointsMax;
            }
            // TEMPORARY
            //hitPoints += 10;
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
            velocity.y -= 0.02f;
        }
        // Controlled: ------------------------------------------CONTROLLED------------------------------------------------------
        if (currentState == Action.NEUTRAL && !airborne)
        {
            // Defending ======================================================
            if (defend)
            {
                defending = true;
                if (canAction)
                {
                    // Backstep
                    if (movementX != 0 && movementX != facing)
                    {
                        velocity = new Vector2(-0.12f * facing, 0.1f);
                        ExecuteAction(MaterialSkillBasic.Roll);
                        actionPoints -= 1;
                    }
                    // Forwardstep
                    if (movementX != 0 && movementX == facing)
                    {
                        velocity = new Vector2(0.2f * facing, 0.0f);
                        ExecuteAction(MaterialSkillBasic.Roll);
                        actionPoints -= 1;
                    }
                    // Sidestep
                    if (movementY != 0)
                    {
                        velocity = new Vector2(0, 0.15f);
                        ExecuteAction(MaterialSkillBasic.Roll);
                        if (movementY >= 1 && position.lane < BattleManager.Manager.field.lanes - 1)
                        {
                            position.lane += 1;
                        }
                        else if (movementY <= -1 && position.lane > 0)
                        {
                            position.lane -= 1;
                        }
                        actionPoints -= 1;
                    }
                }
            }
            else
            {// Not Defending(Normal Movement) ================================
                defending = false;
                // Moving
                if (movementX != 0)
                {
                    facing = movementX;
                    if (movementX >= 1)
                    {
                        position.x += moveSpeed;
                    }
                    else if (movementX <= -1)
                    {
                        position.x -= moveSpeed;
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
        }
        // Using a Basic Attack ---------------------------------ATTACKING-------------------------------------------------------
        if (attackBasic && canAction)
        {
            Debug.Log("Basic attack Input!");
            ExecuteAction(MaterialSkillBasic.BasicAttack);

            actionPoints -= 1;
        }
        // Using a Mat Skill ---------------------------------------------
        if (matSkill && canAction)
        {
            // Side attack
            if (movementX != 0)
            {
                facing = movementX;
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

            actionPoints -= 1;
        }
        // While Attacking(Acting) ==============================WHILEACTING=====================================================
        if (currentState == Action.ACTING)
        {
            //make hits on strike frames If action is an atttack
            if (currentActionFrames.strikeFrames.Length > 0)
            {
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
                                    DamageCVars damageV = new DamageCVars();
                                    damageV.amount = Mathf.Round(currentAttack.effects.damageRatio * attackDamage);
                                    damageV.attackerAcc = accuracy;

                                    other.GetHit(currentAttack.effects.push * facing, currentAttack.effects.lift, damageV);
                                }
                            }
                        }
                    }
                }
            }
            // canAttack in combo frames
            if (execution > currentActionFrames.execution && execution <= currentActionFrames.comboLimit)
            {
                canAttack = true;
            }
            else
            {
                canAttack = false;
            }
            execution++;
        }
        // UnControlled2 ----------------------------------------UNCONTROLLED2---------------------------------------------------
        // Push other Battlers
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
                        position.x -= ((compWidth - Mathf.Abs(xDif)) / 4);
                    }
                    else
                    {
                        position.x += ((compWidth - Mathf.Abs(xDif)) / 4);
                    }
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
        softZ += (position.z - thisTransform.position.z) * 0.2f;
        thisTransform.position = new Vector3(position.x, position.y, softZ);
        // Test Stuff
    }
//---------------------------------------------------------------GETHIT----------------------------------------------------------
    public float GetHit(float xPush, float yPush, DamageCVars a_damage)
    {
        float finalDamage = CalcDamage(a_damage);
        if (finalDamage > 0)
        {
            hitPoints -= finalDamage;
            endurance -= finalDamage;
            if (endurance <= 0 || currentState == Action.STAGGERED)
            {
                Stagger();
            }
            if (currentState == Action.STAGGERED)
            {
                velocity = new Vector2(xPush, yPush) / body.weight;
                Stagger();
            }
        }

        BattleManager.Manager.TraceDamage(center + new Vector3(0, Random.Range(0, (body.height / 2)), 0), finalDamage);
        comboHits++;
        comboDamage += finalDamage;
        comboTime = 1.5f;

        return finalDamage;
        //hitEffect = 1;
    }
    void ExecuteAction(MaterialSkillBasic p_attack)
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

    void Stagger()
    {
        action = Mathf.Max(2, 20 - comboHits);
        execution = 0;
        currentState = Action.STAGGERED;
        defending = false;
        thisAnimation.Play("hurt", -1, 0);
    }

    float CalcDamage(DamageCVars a_damage)
    {
        float AccEvaMod = 1;
        float AccEvaScale = (evade + a_damage.attackerAcc) / 2; // 0 + 100 = 100
        AccEvaMod = (((evade - a_damage.attackerAcc) / evade) / 2) * -1; // 0 - 100 / 100 = -1
        if (defending && AccEvaMod > 0)
        {
            AccEvaMod = 0;
        }
        if (currentState == Action.STAGGERED && AccEvaMod < 0)
        {
            AccEvaMod = 0;
        }

        float armorMod = 1 - (armor / (armor + 100));

        float blockMod = 1;
        if (defending)
        {
            blockMod = 0.25f;
        }
        Debug.Log(evade+"-"+a_damage.attackerAcc+"="+(evade - a_damage.attackerAcc)+"/"+evade+"= "+AccEvaMod);
        Debug.Log("Damage:" + a_damage.amount + " * Block:" + blockMod + " * ArmorReduction:" + armorMod + " = " + (a_damage.amount * blockMod * armorMod) + "   Hit Damage: +" + a_damage.amount + " * " + AccEvaMod + " = " + (a_damage.amount * AccEvaMod));
        //Debug.Log();
        return Mathf.Round(Mathf.Max((a_damage.amount * blockMod * armorMod) + (a_damage.amount * AccEvaMod), 0));
    }
}
