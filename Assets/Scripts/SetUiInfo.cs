using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUiInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        foreach (Button button in GetComponentInChildren<Transform>().GetComponentsInChildren<Button>())
        {
            if (button.CompareTag("spawnCostButton"))
            {
                button.GetComponentInChildren<Text>().text = "Buy cost \n" + GameManager.buildingReferance.GetComponent<Building>().spawnCost;
            }
            if (button.CompareTag("levelUpCostButton"))
            {
                button.GetComponentInChildren<Text>().text = "Level Up cost \n" + GameManager.buildingReferance.GetComponent<Building>().spawnCost;
            }
        }
    }

    private void OnDisable()
    {
        GameManager._instance.isBuildingMenu = false;
    }

    public void UpdateButtonInfo(Button button)
    {
         if (button.CompareTag("spawnCostButton"))
         {
             button.GetComponentInChildren<Text>().text = "Buy cost \n" + GameManager.buildingReferance.GetComponent<Building>().spawnCost;
         } 
         if (button.CompareTag("levelUpCostButton"))
         {
             button.GetComponentInChildren<Text>().text = "Level Up cost \n" + GameManager.buildingReferance.GetComponent<Building>().spawnCost;
         }
    }
}
