using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CameraTranslate cam = null;
    [SerializeField] private GameObject userCampPanel = null;
    [SerializeField] private GameObject enemyCampPanel = null;
    [SerializeField] private GameObject upgradePanel = null;
    [SerializeField] private GameObject gameWinPanel = null;
    [SerializeField] private GameObject gameOverPanel = null;
    [SerializeField] private GameObject guideText = null;
    [SerializeField] private GameObject useMoneyEffectPrefab = null;
    [SerializeField] private GameObject skipButton = null;
    #region InfoBar
    [SerializeField] private Text timerText = null;
    [SerializeField] private Text moneyText = null;
    [SerializeField] private Text totalTankCntText = null;
    [SerializeField] private Text enemyTotalTankCntText = null;
    #endregion

    #region UpgradePanel
    [SerializeField] private UpgradeButtonPanel upgradeBtnPanel = null;
    [SerializeField] private TankInfoPanel tankInfoPanel = null;
    [SerializeField] private TankInfoPanel debugEnemyTankInfoPanel = null;
    #endregion

    private Stats userStats = null;
    private Stats enemyStats = null;

    private GameObject canvas = null;

    private void Start()
    {
        canvas = GameObject.Find("Canvas");

        upgradeBtnPanel.SetStats(userStats);
        tankInfoPanel.SetStats(userStats);
        debugEnemyTankInfoPanel.SetStats(enemyStats);
        upgradePanel.SetActive(false);
        enemyCampPanel.SetActive(false);
    }

    public void SetTimerText(int _value)
    {
        timerText.text = _value.ToString("D2");
    }

    public void SetMoneyText(int _value)
    {
        moneyText.text = string.Format("{0:N0}", _value);
    }

    public void SetTotalTankCntText(int _value)
    {
        totalTankCntText.text = _value.ToString("D2");
    }

    public void SetEnemyTotalTankCntText(int _value)
    {
        enemyTotalTankCntText.text = _value.ToString("D2");
    }

    public void GoToEnemyCamp()
    {
        cam.GoToEnemyCamp();
        userCampPanel.SetActive(false);
        StartCoroutine("LazyActivePanel", enemyCampPanel);
    }

    public void GoToUpgradeCamp()
    {
        cam.GoToUpgrade();
        userCampPanel.SetActive(false);
        StartCoroutine("LazyActivePanel", upgradePanel) ;
    }

    public void UpdateUpgradeUI(int _money)
    {
        upgradeBtnPanel.CheckInteractable(_money);
        upgradeBtnPanel.SetNextCost();
        tankInfoPanel.SetNewStatText();
    }

    public void UpdateDebugUI()
    {
        debugEnemyTankInfoPanel.SetNewStatText();
    }

    public void GoToUserCamp()
    {
        cam.GoToUserCamp();
        StartCoroutine("LazyActivePanel", userCampPanel);
        enemyCampPanel.SetActive(false);
        upgradePanel.SetActive(false);
    }

    public void SetStats(Stats _userStats, Stats _enemyStats)
    {
        if (_userStats == null || _enemyStats == null) return;
        userStats = _userStats;
        enemyStats = _enemyStats;
    }

    private IEnumerator LazyActivePanel(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        obj.SetActive(true);
    }

    public void GameWin()
    {
        gameWinPanel.SetActive(true);
        upgradePanel.SetActive(false);
        userCampPanel.SetActive(false);
        enemyCampPanel.SetActive(false);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        upgradePanel.SetActive(false);
        userCampPanel.SetActive(false);
        enemyCampPanel.SetActive(false);
    }

    public void BlinkGuideText()
    {
        StartCoroutine("Blink", guideText);
    }

    private IEnumerator Blink(GameObject _go)
    {
        for (int i = 0; i < 3; i++)
        {
            _go.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _go.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void UseMoneyEffect(int _value)
    {
        UseMoneyEffect go = Instantiate(useMoneyEffectPrefab).GetComponent<UseMoneyEffect>();
        go.SetText(_value);
        go.transform.SetParent(canvas.transform, false);
    }

    public void ToggleSkipButtion()
    {
        skipButton.SetActive(!skipButton.activeSelf);
    }
}
