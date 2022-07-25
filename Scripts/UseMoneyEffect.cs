using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseMoneyEffect : MonoBehaviour
{
    private Animation anim;
    private Text text;

    private void Awake()
    {
        anim = GetComponent<Animation>();
        text = GetComponent<Text>();
    }

    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    public void SetText(int _money)
    {
        if (_money < 0)
        {
            text.text = _money.ToString();
            text.color = Color.red;
        }
        else
        {
            text.text = "+" + _money.ToString();
        }
    }
}
