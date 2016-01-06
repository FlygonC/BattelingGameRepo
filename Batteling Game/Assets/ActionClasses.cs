﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class ActionFrameData
{
    public int execution;
    public int combo;
    public int lag;
    public int[] strikeFrames;

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
            ret.lag = 30;
            ret.strikeFrames = new int[1] { 15 };

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
    public float damage;
    public float lift;
    public float push;

    public static AttackEffects BasicAttack
    {
        get
        {
            AttackEffects ret = new AttackEffects();
            ret.damage = 5;
            ret.lift = 0;
            ret.push = 0;

            return ret;
        }
    }
}
[System.Serializable]
public class AttackParamaters
{
    private Battler AttackUser;
    public Battler attackUser
    {
        set
        {
            AttackUser = value;
        }
    }
    public AttackEffects effects;
    
    //public AttackHitBox hitbox;
    public ActionFrameData frameData;
    public string animationName = "basicAttack";

    public static AttackParamaters BasicAttack
    {
        get
        {
            AttackParamaters ret = new AttackParamaters();
            ret.frameData = ActionFrameData.BasicAttack;
            ret.effects = AttackEffects.BasicAttack;
            ret.animationName = "basicAttack";

            return ret;
        }
    }
}