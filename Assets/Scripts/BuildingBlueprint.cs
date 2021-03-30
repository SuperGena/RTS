using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlueprint : MonoBehaviour
{
    RaycastHit hit;

    public GameObject spawnPrefab;

    public float spawnCost;

    private void Start()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 50000f, (1<<8)))
        {
            transform.position = hit.point;
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 50000f, (1 << 8)))
        {
            transform.position = hit.point;
        }

        if (Input.GetMouseButton(0))
        {
            Instantiate(spawnPrefab, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}
