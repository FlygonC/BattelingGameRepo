using UnityEngine;
using System.Collections;

[System.Serializable]
public class BAStrike
{
    public int frame;

    public float power = 0;
    public float push = 0;
    public float lift = 0;
}
public class BAMovement
{
    public int frame;

    public float forward = 0;
    public float jump = 0;
}
[System.Serializable]
public class BattlerAction
{
    public int executionFrames;
    public int reposteFrames;
    public BAStrike[] strikeFrames;
    public BAMovement[] movementFrames;

    public int fullFrames
    {
        get
        {
            return executionFrames + reposteFrames;
        }
    }
    public int comboLimit
    {
        get
        {
            return executionFrames + reposteFrames;
        }
    }
    
    public string animationName = "basicAttack";

    public static BattlerAction BasicAttack
    {
        get
        {
            BattlerAction ret = new BattlerAction();
            ret.executionFrames = 15;
            ret.reposteFrames = 15;

            BAStrike strk1 = new BAStrike();
            strk1.frame = 15;
            strk1.power = 0.5f;
            ret.strikeFrames = new BAStrike[1] { strk1 };

            ret.movementFrames = new BAMovement[0];
            ret.animationName = "basicAttack";

            return ret;
        }
    }
    public static BattlerAction Roll
    {
        get
        {
            BattlerAction ret = new BattlerAction();
            ret.executionFrames = 20;
            ret.reposteFrames = 0;
            ret.strikeFrames = new BAStrike[0];
            ret.movementFrames = new BAMovement[0];
            ret.animationName = "jump";

            return ret;
        }
    }

    /*public static BattlerAction SlowAttack
    {
        get
        {
            BattlerAction ret = new BattlerAction();
            ret.frameData = ActionFrameData.SlowAttack;
            ret.effects = AttackEffects.HeavyAttack;
            ret.animationName = "basicAttack";

            return ret;
        }
    }*/
}