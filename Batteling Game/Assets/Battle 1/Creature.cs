using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*[System.Serializable]
public class CrueatureStat
{
    public float _Base = 0;
    public float baseValue
    {
        get { return _Base; }
        set
        {
            _Base = value;
            Calculate();
        }
    }

    public float _FlatBonus = 0;
    public float flatBonus
    {
        get { return _FlatBonus; }
        set 
        { 
            _FlatBonus = value;
            Calculate();
        }
    }

    public float _PercentageBonus = 0;
    public float percentageBonus
    {
        get { return _PercentageBonus; }
        set
        {
            _PercentageBonus = value;
            Calculate();
        }
    }

    public float _FinalBonus;
    public float finalBonus
    {
        get { return _FinalBonus; }
    }
    public float _Final;
    public float final
    {
        get { return _Final; }
    }

    private void Calculate()
    {
        _FinalBonus = flatBonus + (flatBonus * percentageBonus) + (baseValue * percentageBonus);
        _Final = baseValue + finalBonus;
    }
}*/

[System.Serializable]
public class CreatureStats
{
    public float _MaxHealth = 0;
    public float maxHealth
    {
        get { return _MaxHealth; }
    }
    public float _Regeneration = 0;
    public float regeneration
    {
        get { return _Regeneration; }
    }

    public float _Size = 0;
    public float size
    {
        get { return _Size; }
    }

    public float _MoveSpeed = 0;
    public float moveSpeed
    {
        get { return _MoveSpeed; }
    }

    public float _Weight = 0;
    public float weight
    {
        get { return _Weight; }
    }

    public float _Armor = 0;
    public float armor
    {
        get { return _Armor; }
    }


    public static CreatureStats operator +(CreatureStats first, CreatureStats other)
    {
        CreatureStats temp = new CreatureStats();
        temp._MaxHealth = first.maxHealth + other.maxHealth;
        temp._Size = first.size + other.size;
        temp._MoveSpeed = first.moveSpeed + other.moveSpeed;
        temp._Weight = first.weight + other.weight;
        temp._Armor = first.armor + other.armor;

        return temp;
    }

    public CreatureStats()
    {

    }
}

[System.Serializable]
public class EquipmentSlot
{
    public EquipSlot slot;
    public EquipmentItem equiped;

    public EquipmentSlot(EquipSlot a_slot)
    {
        slot = a_slot;
    }
}

[System.Serializable]
public class Creature : MonoBehaviour {
    [Header("Creature Stats")]

    public CreatureStats _BaseStats;
    public CreatureStats stats;

    public float _Health = 100;
    public float health
    {
        get { return _Health; }
        set
        {
            _Health = Mathf.Clamp(value, 0, stats._MaxHealth);
        }
    }


    [Header("Equipment")]
    public WeaponItem _EquipedWeapon;
    public WeaponItem equipedWeapon
    {
        get { return _EquipedWeapon; }
        set { _EquipedWeapon = value; }
    }

    public List<EquipmentSlot> _Equipment = new List<EquipmentSlot>();
    
    
    //Technical variables
    private Creature _Target;
    public Creature target
    {
        get { return _Target; }
        set
        {
            if (value != _Target)
            {
                if (_AttackTime > _LastAttackTime / 2)
                {
                    _AttackTime = 0;
                }
                _Target = value;
            }
        }
    }

    private float _AttackTime = 0;
    public float attackTime { get { return _AttackTime; } }
    private float _LastAttackTime;
    private bool _Strike = false;
    private float _Mobility = 1;
    public float mobility
    {
        get { return _Mobility; }
        private set { _Mobility = Mathf.Clamp(value, 0, 1); }
    }

    private float _RegenTime = 0;

    private Rigidbody2D thisBody;
    private CircleCollider2D thisCollider;
    //private SpriteRenderer thisSprite;
    private Animator thisAnims;

	// Use this for initialization
	void Start () {
        thisBody = this.GetComponent<Rigidbody2D>();

        if (thisBody == null)
        {
            Debug.LogError("Could not find RigidBadoy2D component on " + this.gameObject.name);
        }
        else
        {
            //thisBody.mass = weight / 10;
            thisBody.drag = stats.weight;
        }


        thisCollider = this.GetComponent<CircleCollider2D>();
        if (thisCollider == null)
        {
            Debug.LogError("Could not find CircleCollider2D component on " + this.gameObject.name);
        }
        else
        {
            thisCollider.radius = stats.size;
        }

        //thisSprite = this.GetComponent<SpriteRenderer>();
        thisAnims = this.GetComponent<Animator>();

        _Equipment.Add(new EquipmentSlot(EquipSlot.Head));
        _Equipment.Add(new EquipmentSlot(EquipSlot.Chest));
        _Equipment.Add(new EquipmentSlot(EquipSlot.Arm));
        _Equipment.Add(new EquipmentSlot(EquipSlot.Arm));
        _Equipment.Add(new EquipmentSlot(EquipSlot.Belt));
        _Equipment.Add(new EquipmentSlot(EquipSlot.Leg));
        _Equipment.Add(new EquipmentSlot(EquipSlot.Leg));
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Attacking!
        if (_AttackTime > 0)
        {
            _AttackTime -= Time.deltaTime * 60;
            _Mobility = 0.5f;
            if (_AttackTime <= _LastAttackTime / 2 && _Strike == false)
            {
                //when creature strikes
                _Strike = true;
                target.Damage(this.equipedWeapon.damage);
            }
        }//when attack ends
        else if (_AttackTime <= 0 && _Strike == true)
        {
            _AttackTime = 0;
            _LastAttackTime = 0;
            _Strike = false;
            _Mobility = 1;
            this.thisAnims.Play("Idle");
        }

        //Regeneration
        _RegenTime += stats.regeneration * Time.deltaTime;
        while (_RegenTime >= 1)
        {
            _RegenTime -= 1;
            if (health < stats.maxHealth)
            {
                health++;
            }
        }
        /*if (health > stats.maxHealth)
        {
            health = stats.maxHealth;
        }*/
	}

    void FixedUpdate()
    {
        thisBody.drag = stats.weight;
        thisCollider.radius = stats.size;

        if (health == 0) //Death
        {
            Destroy(this.gameObject);
        }

        if (_EquipedWeapon)
        {
            if (this.GetComponent<CircleDraw>())
            {
                this.GetComponent<CircleDraw>().radius = this.stats.size + this.equipedWeapon.range;
            }
        }
    }

    public bool BasicAttack()
    {
        if (_AttackTime > 0 || target == this || _EquipedWeapon == null)
        {
            return false;
        }
        
        if (TargetinRange())
        {
            _AttackTime = 60 / this.equipedWeapon.attackSpeed;
            _LastAttackTime = 60 / this.equipedWeapon.attackSpeed;
            _Strike = false;

            //Push(1000);
            thisAnims.speed = this.equipedWeapon.attackSpeed;
            //this.thisAnims.Play("Idle");
            this.thisAnims.Play("Attack");

            Debug.Log(this.name + " is attacking " + target.name + "!");

            return true;
        }
        else
        {
            return false;
        }
    }

    public void EquipWeapon(WeaponItem a_weapon)
    {
        _EquipedWeapon = a_weapon;
        /*if (_EquipedWeapon != null)//Stat setting
        {
            _AttackDamage = _EquipedWeapon.attackDamage;
            _AttackSpeed = _EquipedWeapon.attackSpeed;
            _AttackRange = _EquipedWeapon.range;
            //_Weight.flatBonus = _EquipedWeapon.weight;
        }*/
    }

    public bool EquipArmor(EquipmentItem a_armor)
    {
        foreach (EquipmentSlot i in _Equipment)
        {
            if (i.slot == a_armor.slot)
            {
                if (i.equiped == null)
                {
                    i.equiped = a_armor;
                    UpdateStats();
                    return true;
                }
            }
        }
        return false;
    }

    void UpdateStats()
    {
        CreatureStats statsCalc = new CreatureStats(); ;
        foreach (EquipmentSlot i in _Equipment)
        {
            if (i.equiped != null)
            {
                statsCalc += i.equiped.statsAdded;
            }
        }
        stats = _BaseStats + statsCalc;
    }

    bool TargetinRange()
    {
        Vector2 thisPos = (Vector2)this.transform.position;
        Vector2 targetPos = (Vector2)target.transform.position;
        if (Mathf.Abs((thisPos - targetPos).magnitude) <= this.equipedWeapon.range + stats.size + target.stats.size)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Damage(Damage a_damage)
    {
        foreach (SimpleDamage i in a_damage.damages)
        {
            switch (i.type)
            {
                case DamageType.Blade:
                    this.health -= Mathf.Max(0, i.damage - this.stats.armor);
                    break;
                case DamageType.Piercing:
                    this.health -= Mathf.Max(0, i.damage);
                    break;
                case DamageType.Force:
                    this.health -= Mathf.Max(0, i.damage - this.stats.armor);
                    break;
                case DamageType.Energy:
                    this.health -= Mathf.Max(0, i.damage);
                    break;
                default:
                    break;
            }
        }
    }

    void Push(float a_force)
    {
        Vector2 thisPos = (Vector2)this.transform.position;
        Vector2 targetPos = (Vector2)target.transform.position;
        target.GetComponent<Rigidbody2D>().AddForce((targetPos - thisPos).normalized * a_force);
    }
}
