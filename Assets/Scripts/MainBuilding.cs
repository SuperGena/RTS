using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBuilding : MonoBehaviour
{
    public Slider slider;

    public void GetDamage(float amount)
    {
        GameManager.MainBuildingHealth -= amount;
        slider.value = GameManager.MainBuildingHealth;
    }
}
