using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    public GameObject[] buildingBlueprint;


    public void SpawnBlueprint(int ChooseBlueprint)
    {
        if (GameManager.PlayerMoney >= 20f)
        {
            _ = Instantiate(buildingBlueprint[ChooseBlueprint]);
        }
        else
        {
            GameManager.ActivateErrorUI();
        }
    }
}
