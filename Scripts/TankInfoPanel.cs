using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankInfoPanel : MonoBehaviour
{
    private Stats refStats = null;

    private Text[] texts = null;

    private void Awake()
    {
        texts = GetComponentsInChildren<Text>();
    }

    public void SetNewStatText()
    {
        if (refStats == null) return;

        texts[1].text = refStats.AtkDamage.ToString("F0");
        texts[2].text = refStats.AtkSpeed.ToString("F1") + "√ ";
        texts[3].text = refStats.AtkRange.ToString("F1");
        texts[4].text = refStats.MaxHp.ToString("F0");
        texts[5].text = refStats.Armor.ToString("F0");
    }

    public void SetStats(Stats _stats)
    {
        refStats = _stats;
    }
}
