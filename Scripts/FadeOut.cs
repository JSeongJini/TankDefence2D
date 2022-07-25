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
        //현재 스프라이트 렌더러 컬러의 투명도를 1(255)로 설정
        nextColor = sr.color;
        nextColor.a = 1f;
        sr.color = nextColor;

        Invoke("OnFadeOut", 1f);
    }

    void Update()
    {
        //매 프레임마다 스프라이트 렌더러 컬러의 투명도를 조금씩 낮춤
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
