using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour
{
    private float speed = 1f;

    private Vector3 dir = Vector3.zero;
    private bool isMoving = false;

    public event Action onStop;
    public event Action onArrive;

    private void Update()
    {
        if (isMoving)
        {
            transform.Translate(dir * speed * Time.deltaTime, Space.World);
        }
    }

    public void MoveToDir(Vector3 _dir)
    {
        dir = _dir.normalized;
        isMoving = true;
    }

    public void Stop(){
        isMoving = false;
        onStop?.Invoke();
    }

    public void LookAt(Vector3 _dest)
    {
        Vector3 dir = _dest - transform.position;
        dir.z = 0f;
        dir.Normalize();

        //��ũź��Ʈ�� �̿��ؼ�, �� �� ������ ������ ���Ѵ�.
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle -90f);
    }

    public void MoveToDest(Vector3 _dest)
    {
        StopCoroutine("MoveToDestCoroutine");
        StartCoroutine("MoveToDestCoroutine", _dest);
    }

    private IEnumerator MoveToDestCoroutine(Vector3 _dest)
    {
        float distance = Vector3.Distance(transform.position, _dest);
        LookAt(_dest);

        isMoving = true;
        
        //��ǥ�������� �Ÿ��� 0.1�̸��� �� �� ���� �̵�
        while (distance >= 0.1f)
        {
            dir = (_dest - transform.position).normalized;
            distance = Vector3.Distance(transform.position, _dest);
            yield return null;
        }
        isMoving = false;

        onArrive?.Invoke();
    }
}
