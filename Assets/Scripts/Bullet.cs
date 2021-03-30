using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    public float attackPower = 10f;

    public Transform target;

    private void Awake()
    {
        target = GameManager.curentEnemyTarget.transform;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce((target.position - transform.position) * speed, ForceMode.Force);
        
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Enemy>())
        {
            if (other.GetComponent<Player>())
            {
                other.GetComponent<Player>().TakeSomeDamage(attackPower);
            }
            else
            if (other.GetComponent<MainBuilding>())
            {
                other.GetComponent<MainBuilding>().GetDamage(attackPower);
            }
            Destroy(gameObject);
        }
    }
}
