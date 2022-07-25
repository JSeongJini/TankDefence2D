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
            //공격해오는 상대 탱크 수 +(1~2)만큼 탱크 스폰
            _count = opposite.GetTankCount();
            _count += Random.Range(1, 3);
        }

        if (_count > 18) _count = 18;     //현재 맵에서 최대로 설치 가능한 탱크 수 18

        while(tankList.Count < _count)
        {
            //돈이 부족하면 목표 수만큼 스폰을 못하더라도 종료
            if (CheckSpawnMoney() == false) break;
            
            //랜덤으로 스폰위치 생성
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
        //더 이상 업그레이드 할 수 있는 돈이 없을 때 까지 업그레이드
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
