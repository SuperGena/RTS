using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Player : MonoBehaviour
{
    public static Camera cam;

    public NavMeshAgent agent;


    [SerializeField]
    public GameObject targetToDestroy;


    
    public float maxHealth = 30f;
    public float currentHealth = 30f;

    public float maxAttackPower = 10f;
    public float currentAttackPower = 10f;

    public float distanceToAttack;
    public float playerSearchRadius = 5f;
    public float timer = 3f;
    public float waitTime;
    public float waitTimeForEnemyCheck;

    bool isAttack = false;


    public float Health
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            if (value <= 0)
                Destroy(gameObject);
        }
    }

    public virtual void Start()
    {
        cam = Camera.main;
    }

    public virtual void Update()
    {
        if (GameManager.isGameOver)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            if (gameObject.GetComponent<SelectedObject>())

                if (GameManager._instance.isFolowPath)
                {
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if(!hit.transform.GetComponent<Player>())
                            agent.SetDestination(hit.point);
                    }
                    
                }
        }

        if (isAttack)
        {
            if (targetToDestroy == null)
            {
                ReloadTarget();
            }
            else
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
        }
        else
        {
            if (Time.time >= waitTimeForEnemyCheck + 0.1f)
            {
                CheckNearEnemy();
                waitTimeForEnemyCheck = Time.time;
            }
        }
    }

    public void ReloadTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerSearchRadius, 1 << 7);

        if (hitColliders.Length > 0)
        {
            isAttack = true;

            targetToDestroy = hitColliders[0].gameObject;

            foreach (var hitCollider in hitColliders)
            {
                if (Vector3.Distance(transform.position, hitCollider.transform.position) < Vector3.Distance(transform.position, targetToDestroy.transform.position))
                {
                    targetToDestroy = hitCollider.gameObject;
                }
            }

            agent.stoppingDistance = distanceToAttack;
            agent.SetDestination(targetToDestroy.transform.position);
        }
        else
        {
            isAttack = false;

            agent.stoppingDistance = 0f;
            agent.SetDestination(transform.position);
        }
    }

    void CheckNearEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerSearchRadius, 1 << 7);

        if (hitColliders.Length > 0)
        {
            ReloadTarget();
        }

    }

    public void SetParameters(float attack, float health)
    {
        maxAttackPower = attack;
        maxHealth = health;

        currentAttackPower = attack;
        currentHealth = health;
    }

    public void TakeSomeDamage(float amount)
    {
        Health -= amount;
    }

    public void TakeHeal(float amount)
    {
        if((Health + amount) < maxHealth)
        Health += amount;
    }

    public virtual void Attack()
    {
        
    }

    public virtual void OnDestroy()
    {
        GameManager.checkForNewTarget = true;
    }
}
