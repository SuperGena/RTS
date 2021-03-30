using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    public GameObject targetToDestroy;

    public NavMeshAgent navMeshAgent;


    public bool reloadPlayers = false;

    public static float enemyMaxHelth = 20f;

    public float enemyHelth;


    public float attackPower;
    public float distanceToAttack;
    public float playerSearchRadius = 5f;
    public float timer = 3f;
    public float waitTime;
    public float waitTimeForPlayerCheck;

    public float EnemyHelth
    {
        get
        {
            return enemyHelth;
        }
        set
        {
            enemyHelth = value;
            if (enemyHelth <= 0)
                Destroy(gameObject);
        }
    }

    public virtual void Awake()
    {
        waitTime = Time.time;

        if (navMeshAgent != null)
            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

    }

    public virtual void Start()
    {
        waitTimeForPlayerCheck = Time.time - 1f;

        enemyHelth = enemyMaxHelth;

        targetToDestroy = GameManager.curentEnemyTarget;

        navMeshAgent.SetDestination(targetToDestroy.transform.position);

    }

    public virtual void Update()
    {
        if (GameManager.isGameOver)
            return;

        reloadPlayers = GameManager.checkForNewTarget;
        if (GameManager.checkForNewTarget)
        {
            ReloadTarget();

            GameManager.checkForNewTarget = false;
        }

        if (Time.time >= waitTimeForPlayerCheck + 0.1f)
        {
            CheckNearPlayer();
            waitTimeForPlayerCheck = Time.time;
        }

    }

    public void ReloadTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerSearchRadius, 1 << 6);

        if (hitColliders.Length > 0)
        {
            targetToDestroy = hitColliders[0].gameObject;

            foreach (var hitCollider in hitColliders)
            {
                if (Vector3.Distance(transform.position, hitCollider.transform.position) < Vector3.Distance(transform.position, targetToDestroy.transform.position))
                {
                    targetToDestroy = hitCollider.gameObject;
                }
            }

            navMeshAgent.SetDestination(targetToDestroy.transform.position);
        }
        else
        {
            targetToDestroy = GameManager.curentEnemyTarget;

            navMeshAgent.SetDestination(targetToDestroy.transform.position);
        }
    }

    void CheckNearPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerSearchRadius, 1 << 7);

        if (hitColliders.Length > 0)
        {
            ReloadTarget();
        }

    }

    public virtual void Attack()
    {

    }

    public void TakeDamage(float amount)
    {
        EnemyHelth -= amount;
    }

    public virtual void OnDestroy()
    {
        GameManager.enemyDictionary.Remove(gameObject.GetInstanceID());
        GameManager._instance.CheckForWaveClearence();
    }
}
