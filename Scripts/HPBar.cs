using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    private float oriWidth = 0f;

    private RectTransform parentRectTr = null;
    private RectTransform frontRectTr = null;

    private Transform target = null;
    private Vector3 offset = Vector3.zero;

    private void Awake()
    {
        RectTransform[] rectTrs = GetComponentsInChildren<RectTransform>();
        //부모도 가지고 있으면 0번에 들어옴
        parentRectTr = rectTrs[0];
        frontRectTr = rectTrs[2];
        oriWidth = frontRectTr.sizeDelta.x;      //Width
    }

    private void Update()
    {
        Vector3 worldToScreen = Camera.main.WorldToScreenPoint(target.position);
        
        //화면 해상도에 맞춰 위치 조정
        float ratio = Screen.height / 256f;
        parentRectTr.position = worldToScreen + (offset * ratio);
     }


    public void SetHp(float _maxHp, float _hp)
    {
        float ratio = _hp / _maxHp;

        Vector2 size = frontRectTr.sizeDelta;
        size.x = oriWidth * ratio;
        frontRectTr.sizeDelta = size;
    }

    public void SetTarget(Transform _target, Vector3 _offset)
    {
        target = _target;
        offset = _offset;
    }
}
