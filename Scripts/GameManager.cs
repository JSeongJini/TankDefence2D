using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UserController userController = null;
    [SerializeField] private EnemyAIController enemyAIController = null;
    [SerializeField] private UIManager uiMng = null;
    [SerializeField] private Goal winGoal = null;
    [SerializeField] private Goal gameoverGoal = null;

    private EState state = EState.IDLE;
   
    private int stage = 0;

    private readonly float readyTime = 20f;
    private float readyElapsed = 0f;

    private void Awake()
    {
        userController.SetOpposite(enemyAIController);
        enemyAIController.SetOpposite(userController);

        SetUserCallBack();
        SetEnemyCallBack();

        winGoal.onGoal += GameWin;
        gameoverGoal.onGoal += GameOver;

        uiMng.SetStats(userController.GetStats(), enemyAIController.GetStats());
     }

    private void Start()
    {
        Invoke("TranslateToDefenseReady" ,2f);
    }

    private void Update()
    {
        switch (state) {
            case EState.IDLE:
                break;

            case EState.OFFENSE:
                if (userController.GetTankCount() == 0)
                {
                    TranslateToDefenseReady();
                }
                break;

            case EState.OFFENSE_READY:
                readyElapsed += Time.deltaTime;
                if (readyElapsed >= readyTime)
                {
                    readyElapsed = 0f;
                    TranslateToOffense();
                }
                break;

            case EState.DEFENSE:
                if (enemyAIController.GetTankCount() == 0)
                {
                    TranslateToOffenseReady();
                }
                break;

            case EState.DEFENSE_READY:
                readyElapsed += Time.deltaTime;
                if (readyElapsed >= readyTime)
                {
                    readyElapsed = 0f;
                    TranslateToDefense();
                }
                break;

            case EState.GAMEOVER:
                break;

            case EState.WIN:
                break;
        }
    }

    private void LateUpdate()
    {
        if(state == EState.DEFENSE_READY || state  == EState.OFFENSE_READY)
        {
            uiMng.SetTimerText((int)(readyTime - readyElapsed));
        }
    }

    private void GameWin()
    {
        state = EState.WIN;
        uiMng.GameWin();

        StopAllTanks();
    }

    private void GameOver()
    {
        state = EState.GAMEOVER;
        uiMng.GameOver();

        StopAllTanks();
    }

    private void StopAllTanks()
    {
        userController.SetStateAll(false, true);
        enemyAIController.SetStateAll(false, true);
        userController.StopAllTanks();
        enemyAIController.StopAllTanks();
    }

    private void UpdateTotalTankCntText()
    {
        uiMng.SetTotalTankCntText(userController.GetTankCount());
    }

    private void UpdateEnemyTotalTankCntText()
    {
        uiMng.SetEnemyTotalTankCntText(enemyAIController.GetTankCount());
    }

    private void UpdateMoneyText()
    {
        uiMng.SetMoneyText(userController.GetMoney());
    }

     private void TranslateToOffense() {
        state = EState.OFFENSE;
        userController.Offense();
        enemyAIController.Defense();
        enemyAIController.SetFindingTarget(true);
        enemyAIController.UpgradeStatsByRandom();
        uiMng.ToggleSkipButtion();
    }

    private void TranslateToOffenseReady() {
        state = EState.OFFENSE_READY;
        userController.OffenseReady();
        userController.SetFindingTarget(false);
        userController.SetCanSpawn(false);
        enemyAIController.DefenseReady();
        enemyAIController.SpawnTankForDefense();
        enemyAIController.EarnMoney(stage * 10);

        uiMng.ToggleSkipButtion();
    }

    private void TranslateToDefense() {
        state = EState.DEFENSE;

        userController.Defense();
        userController.SetFindingTarget(true);
        enemyAIController.Offense();
        uiMng.ToggleSkipButtion();
    }

    private void TranslateToDefenseReady() {
        stage++;
        state = EState.DEFENSE_READY;
        userController.DefenseReady();
        userController.SetCanSpawn(true);
        enemyAIController.OffenseReady();
        enemyAIController.SetFindingTarget(false);

        uiMng.BlinkGuideText();
        uiMng.ToggleSkipButtion();
    }

    private void SetUserCallBack()
    {
        userController.onSpawn += () =>
        {
            UpdateTotalTankCntText();
        };
        userController.onLostTank += () =>
        {
            UpdateTotalTankCntText();
        };
        userController.onEarnMoney += (_value) =>
        {
            UpdateMoneyText();
            uiMng.UpdateUpgradeUI(userController.GetMoney());
            uiMng.UseMoneyEffect(_value);
        };
    }

    private void SetEnemyCallBack()
    {
        enemyAIController.onSpawn += () =>
        {
            UpdateEnemyTotalTankCntText();
        };
        enemyAIController.onLostTank += () =>
        {
            UpdateEnemyTotalTankCntText();
        };
        enemyAIController.onEarnMoney += (_value) =>
        {
            uiMng.UpdateDebugUI();
        };
    }

    public void SkipReady()
    {
        readyElapsed += readyTime;
        uiMng.SetTimerText(0);
    }
}
