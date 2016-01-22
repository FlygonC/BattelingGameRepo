using UnityEngine;
using System.Collections;

[System.Serializable]
public class ActionFrameData
{
    public int execution;
    public int combo;
    public int lag;
    public int[] strikeFrames;
    public bool superArmor;

    public int fullFrames
    {
        get
        {
            return execution + combo + lag;
        }
    }
    public int comboLimit
    {
        get
        {
            return execution + combo;
        }
    }

    public static ActionFrameData BasicAttack
    {
        get
        {
            ActionFrameData ret = new ActionFrameData();
            ret.execution = 15;
            ret.combo = 15;
            ret.lag = 5;
            ret.strikeFrames = new int[1] { 15 };
            ret.superArmor = false;

            return ret;
        }
    }
    public static ActionFrameData Roll
    {
        get
        {
            ActionFrameData ret = new ActionFrameData();
            ret.execution = 15;
            ret.combo = 0;
            ret.lag = 0;
            ret.strikeFrames = new int[0];
            ret.superArmor = true;

            return ret;
        }
    }
    public static ActionFrameData SlowAttack
    {
        get
        {
            ActionFrameData ret = new ActionFrameData();
            ret.execution = 45;
            ret.combo = 0;
            ret.lag = 45;
            ret.strikeFrames = new int[1] { 45 };
            ret.superArmor = true;

            return ret;
        }
    }
}
public class AttackHitBox
{
    public float range;
}
[System.Serializable]
public class AttackEffects
{
    //public DamageType
    public float damageRatio;
    public float lift;
    public float push;

    public static AttackEffects BasicAttack
    {
        get
        {
            AttackEffects ret = new AttackEffects();
            ret.damageRatio = 0.4f;
            ret.lift = 0;
            ret.push = 0;

            return ret;
        }
    }
    public static AttackEffects Roll
    {
        get
        {
            AttackEffects ret = new AttackEffects();
            ret.damageRatio = 0;
            ret.lift = 0;
            ret.push = 0;

            return ret;
        }
    }
    public static AttackEffects HeavyAttack
    {
        get
        {
            AttackEffects ret = new AttackEffects();
            ret.damageRatio = 1.2f;
            ret.lift = 0.05f;
            ret.push = 0.1f;

            return ret;
        }
    }
}
[System.Serializable]
public class MaterialSkillBasic
{
    public AttackEffects effects;
    
    //public AttackHitBox hitbox;
    public ActionFrameData frameData;
    public string animationName = "basicAttack";

    public static MaterialSkillBasic BasicAttack
    {
        get
        {
            MaterialSkillBasic ret = new MaterialSkillBasic();
            ret.frameData = ActionFrameData.BasicAttack;
            ret.effects = AttackEffects.BasicAttack;
            ret.animationName = "basicAttack";

            return ret;
        }
    }
    public static MaterialSkillBasic Roll
    {
        get
        {
            MaterialSkillBasic ret = new MaterialSkillBasic();
            ret.frameData = ActionFrameData.Roll;
            ret.effects = AttackEffects.Roll;
            ret.animationName = "jump";

            return ret;
        }
    }

    public static MaterialSkillBasic SlowAttack
    {
        get
        {
            MaterialSkillBasic ret = new MaterialSkillBasic();
            ret.frameData = ActionFrameData.SlowAttack;
            ret.effects = AttackEffects.HeavyAttack;
            ret.animationName = "basicAttack";

            return ret;
        }
    }
}