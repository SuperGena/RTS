using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObject : MonoBehaviour
{
    Color baseColor;

    void Start()
    {
        if (GetComponent<Renderer>() != null)
        {
            baseColor = GetComponent<Renderer>().material.color;
            GetComponent<Renderer>().material.color = Color.red;
            Debug.Log("baseColor " + baseColor);
        }
    }


    private void OnDestroy()
    {
        if (GetComponent<Renderer>() != null)
        {
            GetComponent<Renderer>().material.color = baseColor;
        }
    }
}
