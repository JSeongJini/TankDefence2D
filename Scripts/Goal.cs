using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Goal : MonoBehaviour
{
    public event Action onGoal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��ǥ������ ������
        onGoal?.Invoke();
    }

}
