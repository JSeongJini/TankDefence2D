using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Combat))]
[RequireComponent(typeof(Movement))]
public class Tank : MonoBehaviour
{
    [SerializeField] private GameObject hpBarPrefab = null;
    
    private Combat combat = null;
    public Combat CombatComp { get { return combat; } }
    private Movement movement = null;
    public Movement MovementComp { get { return movement; } }
    private Stats stats = null;

    private bool isOffense = false;
    private bool isReady = true;

    private Combat target = null;
    public Combat Target { get { return target; } set { target = value; } }

    private Vector3 wayPoint = Vector3.zero;
 
    private HPBar hpBar = null;

    private void Awake()
    {
        combat = GetComponent<Combat>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        if (isOffense) OffensePhase();
        else DeffensePhase();
    }

    private void OffensePhase()
    {
        if (wayPoint == Vector3.zero) return;
       
        movement.MoveToDest(wayPoint);
    }

    private void DeffensePhase()
    {
        if (target == null) return;
        if (isReady) return;

        combat.Attack(target);
    }

    public void SetWayPoint(Vector3 _wayPoint)
    {
        wayPoint = _wayPoint;
    }

    public void SetStats(Stats _stats)
    {
        stats = _stats;
        combat.SetStats(_stats);
    }

    public void SetState(bool _isOffenes, bool _isReady)
    {
        isOffense = _isOffenes;
        isReady = _isReady;
    }

    public void MakeHpBar()
    {
        hpBar = Instantiate(hpBarPrefab).GetComponent<HPBar>();
        hpBar.SetTarget(transform, new Vector3(0f, -15f, 0f));

        GameObject mainCanvase = GameObject.Find("Canvas");
        hpBar.transform.SetParent(mainCanvase.transform, false);

        combat.onGetDamage += () =>
        {
            hpBar.SetHp(stats.MaxHp, combat.Hp);
        };
        combat.onDie += () =>
        {
            Destroy(hpBar.gameObject);
        };
    }
}
