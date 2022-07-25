using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats { 
    private float maxHp = 100f;
    private float atkDamage = 10f;
    private float atkSpeed = 1f;
    private float atkRange = 5f;
    private float armor = 0f;

    public class Upgrade
    {
        public int maxHpUpg = 0;
        public int atkDmgUpg = 0;
        public int atkSpdUpg = 0;
        public int atkRngUpg = 0;
        public int armorUpg = 0;

        public static int GetUpgradeCost(int _upg)
        {
            return (_upg * 25) + 50;
        }
    }
    private Upgrade upgrade = new Upgrade();
    public Upgrade GetUpgrade { get { return upgrade; } }

    private readonly float minAtkSpeed = 0.1f;

    public float MaxHp { get { return maxHp; } }
    public float AtkDamage { get { return atkDamage; } }
    public float AtkSpeed { get { return atkSpeed; } }
    public float AtkRange { get { return atkRange; } }
    public float Armor { get { return armor; } }

    public void UpgradeMaxHP(float _value)
    {
        maxHp += _value;
        upgrade.maxHpUpg++;
    }

    public void UpgradeAtkDamage(float _value)
    {
        atkDamage += _value;
        upgrade.atkDmgUpg++;
    }

    public void UpgradeAtkSpeed(float _value)
    {
        atkSpeed -= _value;
        upgrade.atkSpdUpg++;
        if (atkSpeed <= minAtkSpeed) atkSpeed = minAtkSpeed;
    }

    public void UpgradeAtkRange(float _value)
    {
        atkRange += _value;
        upgrade.atkRngUpg++;
    }

    public void UpgradeArmor(float _value)
    {
        armor += _value;
        upgrade.armorUpg++;
    }
}
