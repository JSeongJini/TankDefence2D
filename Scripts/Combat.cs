using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Combat : MonoBehaviour
{
    private Stats stats = null;
    
    private float hp = 0f;
    public float Hp { get { return hp; } }
    private float atkCoolTime = 0f;

    private bool isDie = false;

    public event Action onDie;
    public event Action onAttack;
    public event Action onGetDamage;

    private void Start()
    {
        hp = stats.MaxHp;
        atkCoolTime = stats.AtkSpeed;
    }

    private void Update()
    {
        atkCoolTime += Time.deltaTime;
    }

    public void Attack(Combat _target)
    {
        if (isDie) return;
        if (!_target) return;

        if(atkCoolTime >= stats.AtkSpeed && InAttackRange(_target.transform.position))
        {
            atkCoolTime = 0f;

            _target.GetDamage(stats.AtkDamage);

            onAttack?.Invoke();
        }
    }

    public bool InAttackRange(Vector3 _targetPos)
    {
        return (Vector3.Distance(transform.position, _targetPos) <= stats.AtkRange);
    }

    public void GetDamage(float _damage)
    {
        float adjustDmg = _damage - stats.Armor;
        if (adjustDmg < 0f) adjustDmg = 0f;

        hp -= adjustDmg;
      
        if(hp <= 0f)
        {
            hp = 0f;
            if(!isDie) Die();
            isDie = true;
        }
        else
        {
            onGetDamage?.Invoke();
        }
    }

    public void Die()
    {
        onDie?.Invoke();
        Destroy(gameObject);
    }

    public void SetStats(Stats _stats)
    {
        stats = _stats;
    }

    public void SetHpToMaxHp()
    {
        hp = stats.MaxHp;
    }
}
