using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonPanel : MonoBehaviour
{
    private Stats refStats = null;

    private Text[] texts = null;
    private Button[] buttons = null;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        texts = GetComponentsInChildren<Text>();
    }

    public void CheckInteractable(int _money)
    {
        if (refStats == null) return;
        Stats.Upgrade upgrade = refStats.GetUpgrade;

        buttons[0].interactable = (Stats.Upgrade.GetUpgradeCost(upgrade.atkDmgUpg) <= _money);
        buttons[1].interactable = (Stats.Upgrade.GetUpgradeCost(upgrade.atkSpdUpg) <= _money) && (upgrade.atkSpdUpg < 9);
        buttons[2].interactable = (Stats.Upgrade.GetUpgradeCost(upgrade.atkRngUpg) <= _money);
        buttons[3].interactable = (Stats.Upgrade.GetUpgradeCost(upgrade.maxHpUpg) <= _money);
        buttons[4].interactable = (Stats.Upgrade.GetUpgradeCost(upgrade.armorUpg) <= _money);
    }

    public void SetNextCost()
    {
        if (refStats == null) return;
        Stats.Upgrade upgrade = refStats.GetUpgrade;
        texts[1].text = "$ " + Stats.Upgrade.GetUpgradeCost(upgrade.atkDmgUpg);
        texts[3].text = "$ " + Stats.Upgrade.GetUpgradeCost(upgrade.atkSpdUpg);
        texts[5].text = "$ " + Stats.Upgrade.GetUpgradeCost(upgrade.atkRngUpg);
        texts[7].text = "$ " + Stats.Upgrade.GetUpgradeCost(upgrade.maxHpUpg);
        texts[9].text = "$ " + Stats.Upgrade.GetUpgradeCost(upgrade.armorUpg);
    }

    public void SetStats(Stats _stats)
    {
        refStats = _stats;
    }
  
}
