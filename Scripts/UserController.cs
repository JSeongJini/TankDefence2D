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
            
            //findingTarget == ��������� �� == !isReady
            tank.SetState(false, !findingTarget);

            EarnMoney(-tankSpawnMoney);
        }
    }

    private Vector3 PickSpawnPos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        //Ÿ�ϸ��̱� ������ Ÿ�� ��ġ�� Ŭ�� ������ ��ġ��Ű�� ���� �ݿø� ���
        Vector3 spawnPos = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0f);

        if (CheckSpawnPos(spawnPos))
            return spawnPos;
        else return Vector3.zero;       //Vector3.zero�� �߸��� ������ ���
    }

    public void SetCanSpawn(bool _value) {
        //[ ���� �غ�, ���� ] ������ �� true
        canSpawn = _value;
    }
}
