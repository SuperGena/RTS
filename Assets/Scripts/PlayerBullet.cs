using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
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

        Destroy(gameObject, 8f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().TakeDamage(attackPower);
            Destroy(gameObject);
        }
    }
}
