using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : TankManager
{
    private bool canSpawn = true;

    private void Awake()
    {
        attackPos = new Vector3(-30f, -0.5f, 0f);
    }

    protected override void Update()
    {
        base.Update();

        if (canSpawn) SpawnTankByClick();
    }

    private void SpawnTankByClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPos = PickSpawnPos();
            if (spawnPos == Vector3.zero) return;

            if (!CheckSpawnMoney()) return;

            Tank tank = SpawnTank(spawnPos);
            
            //findingTarget == 수비상태일 때 == !isReady
            tank.SetState(false, !findingTarget);

            EarnMoney(-tankSpawnMoney);
        }
    }

    private Vector3 PickSpawnPos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        //타일맵이기 때문에 타일 위치와 클릭 지점을 일치시키기 위해 반올림 사용
        Vector3 spawnPos = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0f);

        if (CheckSpawnPos(spawnPos))
            return spawnPos;
        else return Vector3.zero;       //Vector3.zero를 잘못된 값으로 사용
    }

    public void SetCanSpawn(bool _value) {
        //[ 수비 준비, 수비 ] 상태일 때 true
        canSpawn = _value;
    }
}
