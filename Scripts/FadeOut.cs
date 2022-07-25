using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FadeOut : MonoBehaviour
{
    private SpriteRenderer sr = null;
    private Color nextColor;

    public event Action onFadeOut = null;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        //���� ��������Ʈ ������ �÷��� ������ 1(255)�� ����
        nextColor = sr.color;
        nextColor.a = 1f;
        sr.color = nextColor;

        Invoke("OnFadeOut", 1f);
    }

    void Update()
    {
        //�� �����Ӹ��� ��������Ʈ ������ �÷��� ������ ���ݾ� ����
        nextColor = sr.color;
        nextColor.a -= Time.deltaTime;
        if (nextColor.a <= 0f) nextColor.a = 0f;
        sr.color = nextColor;
    }

    private void OnFadeOut()
    {
        onFadeOut?.Invoke();
    }
}
