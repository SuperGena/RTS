using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorUI : MonoBehaviour
{
    public float timeTillClose;
    float timer;

    private void OnEnable()
    {
        timer = Time.time;
    }

    private void Update()
    {
        if (Time.time >= timeTillClose + timer)
        {
            gameObject.SetActive(false);
        }
    }
}
