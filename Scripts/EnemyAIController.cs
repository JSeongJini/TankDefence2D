using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : TankManager
{ 
    private void Start()
    {
        attackPos = new Vector3(9f, -0.5f, 0f);
        money = 450;
        SpawnTankForDefense(3);
    }

    public void SpawnTankForDefense(int _count = 0)
    {
        if (_count == 0)
        {
            //�����ؿ��� ��� ��ũ �� +(1~2)��ŭ ��ũ ����
            _count = opposite.GetTankCount();
            _count += Random.Range(1, 3);
        }

        if (_count > 18) _count = 18;     //���� �ʿ��� �ִ�� ��ġ ������ ��ũ �� 18

        while(tankList.Count < _count)
        {
            //���� �����ϸ� ��ǥ ����ŭ ������ ���ϴ��� ����
            if (CheckSpawnMoney() == false) break;
            
            //�������� ������ġ ����
            int x = Random.Range(-27, -9);
            int y = (x % 2 == 0) ? 1 : -2;
            Vector3 spawnPos = new Vector3((float)x, (float)y, 0f);

            if (CheckSpawnPos(spawnPos) == true)
            {
                SpawnTank(spawnPos);
                EarnMoney(-tankSpawnMoney);
            }
        }
    }

    public void UpgradeStatsByRandom()
    {
        //�� �̻� ���׷��̵� �� �� �ִ� ���� ���� �� ���� ���׷��̵�
        int minCost = FindMinCost();
                
        while(money >= minCost)
        {
            int rand = Random.Range(0, 5);

            switch (rand)
            {
                case 0:
                    UpgradeAtkDamage();
                    break;
                case 1:
                    UpgradeAtkSpeed();
                    break;
                case 2:
                    UpgradeAtkRange();
                    break;
                case 3:
                    UpgradeMaxHP();
                    break;
                case 4:
                    UpgradeArmor();
                    break;
            }
            minCost = FindMinCost();
        }
     }

    private int FindMinCost()
    {
        int min = Stats.Upgrade.GetUpgradeCost(stats.GetUpgrade.maxHpUpg);
        int cost = Stats.Upgrade.GetUpgradeCost(stats.GetUpgrade.atkDmgUpg);
        if (min > cost) min = cost;
        
        cost = Stats.Upgrade.GetUpgradeCost(stats.GetUpgrade.atkSpdUpg);
        if (min > cost) min = cost;

        cost = Stats.Upgrade.GetUpgradeCost(stats.GetUpgrade.atkRngUpg);
        if (min > cost) min = cost;

        cost = Stats.Upgrade.GetUpgradeCost(stats.GetUpgrade.armorUpg);
        if (min > cost) min = cost;

        return min;
    }
}
