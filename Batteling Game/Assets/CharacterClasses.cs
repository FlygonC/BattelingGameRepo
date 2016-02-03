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
        return Mathf.Round(((_baseHp * _level * 2) / 10) * (_level / 10) + (10 * _level));
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
    public BattlerAction materialNeutral = BattlerAction.BasicAttack;
    public BattlerAction materialUp = BattlerAction.BasicAttack;
    public BattlerAction materialSide = BattlerAction.BasicAttack;
    public BattlerAction materialDown = BattlerAction.BasicAttack;

    public static CharacterSkills TestSkills
    {
        get
        {
            CharacterSkills skills = new CharacterSkills();

            skills.materialNeutral = BattlerAction.BasicAttack;
            skills.materialUp = BattlerAction.BasicAttack;
            
            skills.materialDown = BattlerAction.BasicAttack;

            BattlerAction ret = new BattlerAction();
            ret.executionFrames = 22;
            ret.reposteFrames = 15;

            BAStrike strk1 = new BAStrike();
            strk1.frame = 15;
            strk1.power = 0.7f;
            strk1.push = 0.1f;
            ret.strikeFrames = new BAStrike[1] { strk1 };

            BAMovement move1 = new BAMovement();
            move1.frame = 15;
            move1.forward = 0.1f;
            ret.movementFrames = new BAMovement[1] { move1 };

            ret.animationName = "thrust";

            skills.materialSide = ret;

            return skills;
        }
    }
}