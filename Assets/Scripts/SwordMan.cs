using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordMan : Enemy
{

    public Animator anim;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();
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
            distanceToAttack = 20f;
        else
            distanceToAttack = 5f;
    }

    public override void Attack()
    {
        anim.SetTrigger("Attack");
        if (targetToDestroy == GameManager.curentEnemyTarget)
        {
            distanceToAttack = 25f;
            targetToDestroy.GetComponent<MainBuilding>().GetDamage(attackPower);
        }
        else
        {
            distanceToAttack = 3f;
            targetToDestroy.GetComponent<Player>().TakeSomeDamage(attackPower);
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
