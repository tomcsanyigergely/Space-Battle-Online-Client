using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReinstallTime : MonoBehaviour
{
    public UnityAction<float> onRemainingTimeChanged;

    private float remainingTime = 0;

    public float RemainingTime
    {
        get
        {
            return remainingTime;
        }
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime < 0)
            {
                remainingTime = 0;
            }
            onRemainingTimeChanged(remainingTime);
        }
    }

    public void SetRemainingTime(float remainingTime)
    {
        this.remainingTime = remainingTime;
        onRemainingTimeChanged(remainingTime);
    }
}
