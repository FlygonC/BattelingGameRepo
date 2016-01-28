using UnityEngine;
using System.Collections;

[System.Serializable]
public class Character
{
    //public CharacterExperience experience = new CharacterExperience();
    public int level = 1;
    /*    {
            get
            {
                return experience.level;
            }
            set
            {
                experience.level = value;
            }
        }*/
    public Stats baseStats = new Stats();
    public Stats activeStats = new Stats();
    // Weapon Equip
    // Armor Equip

    public float hpMax
    {
        get { activeStats.hp = Stats.LevelFormulaHp(baseStats.hp, level); return activeStats.hp; }
    }
    public float hp = 100;
    public float mpMax
    {
        get { activeStats.mp = Stats.LevelFormulaMp(baseStats.mp, level); return activeStats.mp; }
    }
    // Base Battle Stats
    public float attackDamage
    {
        get { activeStats.attackDamage = Stats.LevelFormula(baseStats.attackDamage, level); return activeStats.attackDamage; }
    }
    public float magicDamage
    {
        get { activeStats.magicDamage = Stats.LevelFormula(baseStats.magicDamage, level); return activeStats.magicDamage; }
    }
    public float armor = 0;
    public float accuracy
    {
        get { activeStats.accuracy = Stats.LevelFormula(baseStats.accuracy, level); return activeStats.accuracy; }
    }
    public float evasion
    {
        get { activeStats.evasion = Stats.LevelFormula(baseStats.evasion, level); return activeStats.evasion; }
    }

    //public float block = 0;// + weapon block power

    public CharacterSkills skills = CharacterSkills.TestSkills;

    public void UpdateAllStats()
    {
        activeStats.hp =            Stats.LevelFormulaHp(baseStats.hp, level);
        activeStats.mp =            Stats.LevelFormulaMp(baseStats.mp, level);
        activeStats.attackDamage =  Stats.LevelFormula(baseStats.attackDamage, level);
        activeStats.magicDamage =   Stats.LevelFormula(baseStats.magicDamage, level);
        activeStats.accuracy =      Stats.LevelFormula(baseStats.accuracy, level);
        activeStats.evasion =       Stats.LevelFormula(baseStats.evasion, level);
    }
}
[System.Serializable]
public class Stats
{
    public float hp;
    public float mp;
    public float attackDamage;
    public float magicDamage;
    public float accuracy;
    public float evasion;
    /*
    public int vitality = 5;
    public int strength = 5;
    public int magic = 5;
    public int agility = 5;
    */
    public void SetAll(float _num)
    {
        hp = _num;
        mp = _num;
        attackDamage = _num;
        magicDamage = _num;
        accuracy = _num;
        evasion = _num;
    }

    public static float LevelFormulaHp(float _baseHp, float _level)
    {
        return Mathf.Round(((_baseHp * _level * 2) / 10) * (_level / 10) + (4 * _level));
    }
    public static float LevelFormulaMp(float _baseMp, float _level)
    {
        return Mathf.Round(((_baseMp * _level) / 10) * (_level / 20) + (2 * _level));
    }
    public static float LevelFormula(float _baseStat, float _level)
    {
        return Mathf.Round(((_baseStat * _level) / 10) * (_level / 20) + (2 * _level));
    } 
}

public class CharacterExperience
{
    public int level = 1;
    public float EXP = 0;
    public const float EXPcurveConstant = 0.2f;

    public float nextLevel
    {
        get
        {
            return Mathf.Pow(level / EXPcurveConstant, 2);
        }
    }
    public float toNextLevel
    {
        get
        {
            return nextLevel - EXP;
        }
    }
}
public class CharacterSkills
{
    public MaterialSkillBasic materialNeutral = MaterialSkillBasic.BasicAttack;
    public MaterialSkillBasic materialUp = MaterialSkillBasic.BasicAttack;
    public MaterialSkillBasic materialSide = MaterialSkillBasic.BasicAttack;
    public MaterialSkillBasic materialDown = MaterialSkillBasic.BasicAttack;

    public static CharacterSkills TestSkills
    {
        get
        {
            CharacterSkills skills = new CharacterSkills();

            MaterialSkillBasic testAttack1 = new MaterialSkillBasic();
            AttackEffects testEffects1 = new AttackEffects();
            testEffects1.push = 0.1f;
            testEffects1.lift = 0;
            testEffects1.damageRatio = 0.2f;
            ActionFrameData testFrameData1 = new ActionFrameData();
            testFrameData1.execution = 25;
            testFrameData1.combo = 5;
            testFrameData1.lag = 5;
            testFrameData1.strikeFrames = new int[2] { 15, 25 };

            testAttack1.frameData = testFrameData1;
            testAttack1.effects = testEffects1;
            testAttack1.animationName = "thrust";

            skills.materialDown = MaterialSkillBasic.SlowAttack;
            skills.materialSide = testAttack1;
            // UP
            MaterialSkillBasic testAttack2 = new MaterialSkillBasic();
            AttackEffects testEffects2 = new AttackEffects();
            testEffects2.push = 0;
            testEffects2.lift = 0.3f;
            testEffects2.damageRatio = 0.3f;
            ActionFrameData testFrameData2 = new ActionFrameData();
            testFrameData2.execution = 10;
            testFrameData2.combo = 45;
            testFrameData2.lag = 1;
            testFrameData2.strikeFrames = new int[1] { 5 };

            testAttack2.frameData = testFrameData2;
            testAttack2.effects = testEffects2;
            //testAttack2.animationName = "attackBasic";

            skills.materialUp = testAttack2;

            return skills;
        }
    }
}