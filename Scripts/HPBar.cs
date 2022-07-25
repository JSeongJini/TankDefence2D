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
        //�θ� ������ ������ 0���� ����
        parentRectTr = rectTrs[0];
        frontRectTr = rectTrs[2];
        oriWidth = frontRectTr.sizeDelta.x;      //Width
    }

    private void Update()
    {
        Vector3 worldToScreen = Camera.main.WorldToScreenPoint(target.position);
        
        //ȭ�� �ػ󵵿� ���� ��ġ ����
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
