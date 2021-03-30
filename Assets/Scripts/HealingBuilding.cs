using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBuilding : MonoBehaviour
{
    public float playerSearchRadius = 15f;
        
    public float timeTillHeal = 2f;

    public float waitTime;

    public float healAmount;

    void Start()
    {
        
    }

    void Update()
    {
        if (Time.time >= timeTillHeal + waitTime)
        {
            CheckPlayerForHeal();
            waitTime = Time.time;
        }
    }

    void CheckPlayerForHeal()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerSearchRadius, 1 << 6);

        if (hitColliders.Length > 0)
        {
            foreach (var hitCollider in hitColliders)
            {
                hitCollider.GetComponent<Player>().TakeHeal(healAmount);
            }
        }
    }
}
