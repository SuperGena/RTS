using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowMan : Enemy
{
    public GameObject bulletType;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (targetToDestroy == null)
        {
            ReloadTarget();
        }
        if (Vector3.Distance(transform.position, targetToDestroy.transform.position) <= distanceToAttack)
        {
            
            if (Time.time >= waitTime + timer)
            {
                Attack();
                waitTime = Time.time;
            }
        }
        else
        {
            ReloadTarget();
        }

        if (targetToDestroy == GameManager.curentEnemyTarget)
            navMeshAgent.stoppingDistance = 15f;
        else
            navMeshAgent.stoppingDistance = 8f;

    }

    public override void Attack()
    {
        GameObject bullet = Instantiate(bulletType, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().target = targetToDestroy.transform;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

}
