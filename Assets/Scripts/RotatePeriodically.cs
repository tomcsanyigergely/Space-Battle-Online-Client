using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePeriodically : MonoBehaviour
{
    [SerializeField] private float period;
    [SerializeField] private float rotationAngle;

    private float timeToRotate;

    private void Start()
    {
        timeToRotate = period;
    }

    private void Update()
    {
        timeToRotate -= Time.deltaTime;

        if (timeToRotate <= 0)
        {
            transform.Rotate(new Vector3(0, 0, 1), -rotationAngle);
            timeToRotate += period;
        }
    }
}
