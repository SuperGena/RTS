using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MellePlayer : Player
{
    Animator anim;

    public override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        if (targetToDestroy != null)
        {
            anim.SetTrigger("Attack");
            targetToDestroy.GetComponent<Enemy>().TakeDamage(currentAttackPower);
        }
        else
        {
            ReloadTarget();
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
