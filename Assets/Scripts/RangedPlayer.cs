using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedPlayer : Player
{
    [SerializeField]
    public GameObject bulletType;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void Attack()
    {
        GameObject bullet = Instantiate(bulletType, transform.position, Quaternion.identity);
        bullet.GetComponent<PlayerBullet>().attackPower = currentAttackPower;
        bullet.GetComponent<PlayerBullet>().target = targetToDestroy.transform;
    }
}
