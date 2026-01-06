using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] private float timeLimit;

    private TimerManager timerManager;

    void Update()
    {
        timerManager = GetComponent<TimerManager>();

        if (timerManager != null)
        {
            timeLimit = timerManager.timeLimit;
        }
    }
}


