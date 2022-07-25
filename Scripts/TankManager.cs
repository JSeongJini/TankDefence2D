using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TankManager : MonoBehaviour
{
    [SerializeField] GameObject tankPrefab = null;

    protected Stats stats = new Stats();

    protected List<Tank> tankList = new List<Tank>();
    protected int tankSpawnMoney = 50;
    protected int money = 0;
    protected int killMoney = 100;

    protected TankManager opposite = null;

    public event Action onSpawn;
    public event Action onLostTank;
    public delegate void OnEarnMoney(int _money);
    public OnEarnMoney onEarnMoney;

    protected Vector3 attackPos = Vector3.zero;
    protected bool findingTarget = false;


    //탱크의 상태는 [ 공격, 수비, 공격준비, 수비준비 ]로 4 상태가 있다.
    private void Start()
    {
        EarnMoney(200);
    }

    protected virtual void Update()
    {
        if (findingTarget) SetTargetAll();
    }

    protected Tank SpawnTank(Vector3 _spawnPos, bool _isReady = true)
    {
        GameObject tankGo = Instantiate(tankPrefab, _spawnPos, Quaternion.identity);
        tankGo.transform.SetParent(transform);

        Tank tankComp = tankGo.GetComponent<Tank>();
        tankComp.SetStats(stats);
        tankComp.SetState(false, _isReady);
        tankList.Add(tankComp);

        tankComp.CombatComp.onAttack += () =>
        {
            tankComp.MovementComp.LookAt(tankComp.Target.transform.position);
            GameObject effectGo = AttackEffectPool.GetInstance.GetObject();
            effectGo.transform.position = tankComp.transform.position + (tankComp.transform.up * 0.75f);
        };

        tankComp.CombatComp.onGetDamage += () =>
        {
            GameObject effectGo = DamageEffectPool.GetInstance.GetObject();

            Vector3 ranPos = new Vector3(
                UnityEngine.Random.Range(-0.3f, 0.3f),
                UnityEngine.Random.Range(-0.3f, 0.3f),
                0f);
            effectGo.transform.position = tankComp.transform.position + ranPos;
        };

        tankComp.CombatComp.onDie += () =>
        {
            opposite.EarnMoney(killMoney);
            tankList.Remove(tankComp);
            onLostTank?.Invoke();
        };

        onSpawn?.Invoke();

        return tankComp;
    }

    public Tank GetNearTankWithPoint(Vector3 _point)
    {
        if (tankList == null || tankList.Count == 0)
            return null;

        Tank nearTank = tankList[0];
        float neaTankDist = Vector3.Distance(_point, nearTank.transform.position);
        for (int i = 1; i < tankList.Count; i++)
        {
            float unitDist = Vector3.Distance(_point, tankList[i].transform.position);
            if (neaTankDist > unitDist)
            {
                neaTankDist = unitDist;
                nearTank = tankList[i];
            }
        }

        return nearTank;
    }

    public void EarnMoney(int _value)
    {
        money += _value;
        if (money >= 10000) money = 9999;
        onEarnMoney?.Invoke(_value);
    }


    public void SetOpposite(TankManager _opposite)
    {
        opposite = _opposite;
    }

    protected void SetTargetAll()
    {
        if (tankList.Count == 0 || !opposite)
            return;

        foreach (Tank tank in tankList)
        {
            Tank nearTank = opposite.GetNearTankWithPoint(tank.transform.position);
            tank.Target = nearTank?.GetComponent<Combat>();
        }
    }

    protected void SetWayPointAll(Vector3 _wayPoint)
    {
        if (tankList.Count == 0 || !opposite)
            return;

        foreach (Tank tank in tankList)
        {
            tank.SetWayPoint(_wayPoint);
        }
    }

    public void StopAllTanks()
    {
        if (tankList.Count == 0)
            return;

        foreach (Tank tank in tankList)
        {
            tank.MovementComp.Stop();
        }
    }

    public void SetStateAll(bool _isOffense, bool _isReady)
    {
        if (tankList.Count == 0 || !opposite)
            return;

        foreach (Tank tank in tankList)
        {
            tank.SetState(_isOffense, _isReady);
        }
    }

    public bool CheckSpawnPos(Vector3 _pos)
    {
        if (_pos.x > 9f || _pos.x < -29f) return false;

        RaycastHit2D hit = Physics2D.Raycast(_pos, Vector2.zero, 0f);
        if (hit) return false;

        if ((_pos.y == 1f && ((int)_pos.x) % 2 == 0)
            || (_pos.y == -2f && ((int)_pos.x) % 2 != 0))
            return true;
        else
            return false;
    }

    public bool CheckSpawnMoney()
    {
        return money >= tankSpawnMoney;
    }

    public int GetTankCount()
    {
        return tankList.Count;
    }

    public int GetMoney()
    {
        return money;
    }

    public Stats GetStats()
    {
        return stats;
    }

    public void SetKillMoney(int _value)
    {
        killMoney = _value;
    }

    public void Defense()
    {
        SetStateAll(false, false);
    }

    public virtual void DefenseReady()
    {
        SetStateAll(false, true);
    }

    public virtual void Offense()
    {
        SetStateAll(true, false);
        SetWayPointAll(attackPos);
        UpdateHpToMaxHp();

        foreach (Tank tank in tankList)
        {
            tank.MakeHpBar();
        }
    }

    public void OffenseReady()
    {
        foreach (Tank tank in tankList)
        {
            tank.SetWayPoint(new Vector3(tank.transform.position.x, -0.5f, 0f));
        }

        SetStateAll(true, true);
    }

    private void UpdateHpToMaxHp()
    {
        foreach (Tank tank in tankList)
        {
            tank.CombatComp.SetHpToMaxHp();
        }
    }

    public void SetFindingTarget(bool _value)
    {
        findingTarget = _value;
    }

    public void UpgradeMaxHP()
    {
        int cost = Stats.Upgrade.GetUpgradeCost(stats.GetUpgrade.maxHpUpg);
        if (money < cost) return;

        stats.UpgradeMaxHP(10f);
        EarnMoney(-cost);
    }

    public void UpgradeAtkDamage()
    {
        int cost = Stats.Upgrade.GetUpgradeCost(stats.GetUpgrade.atkDmgUpg);
        if (money < cost) return;

        stats.UpgradeAtkDamage(1f);
        EarnMoney(-cost);
    }

    public void UpgradeAtkSpeed()
    {
        int cost = Stats.Upgrade.GetUpgradeCost(stats.GetUpgrade.atkSpdUpg);
        if (money < cost) return;

        stats.UpgradeAtkSpeed(0.1f);
        EarnMoney(-cost);
    }

    public void UpgradeAtkRange()
    {
        int cost = Stats.Upgrade.GetUpgradeCost(stats.GetUpgrade.atkRngUpg);
        if (money < cost) return;

        stats.UpgradeAtkRange(0.5f);
        EarnMoney(-cost);
    }

    public void UpgradeArmor()
    {
        int cost = Stats.Upgrade.GetUpgradeCost(stats.GetUpgrade.armorUpg);
        if (money < cost) return;

        stats.UpgradeArmor(1f);
        EarnMoney(-cost);
    }
}
