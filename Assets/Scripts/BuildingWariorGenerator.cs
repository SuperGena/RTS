using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingWariorGenerator : MonoBehaviour
{

    public GameObject wariorToGenerate;
    public Transform spawnPosition;
    public float generateSpeed;
    public float timer;

    private void Awake()
    {
        spawnPosition = transform.GetComponentInChildren<Transform>();
    }

    void Update()
    {
        if (Time.time >= timer + generateSpeed)
        {
            GameObject warior = Instantiate(wariorToGenerate,spawnPosition.position,Quaternion.identity);
            warior.GetComponent<NavMeshAgent>().Warp(new Vector3( spawnPosition.position.x + Random.Range(-10, 11), spawnPosition.position.y, spawnPosition.position.z + Random.Range(-10, 11)));
            warior.GetComponent<NavMeshAgent>().SetDestination(GameManager.curentEnemyTarget.transform.position);
            timer = Time.time;
        }
    }
}
