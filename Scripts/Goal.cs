using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Goal : MonoBehaviour
{
    public event Action onGoal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //목표지점에 닿으면
        onGoal?.Invoke();
    }

}
