using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity : MonoBehaviour
{
    public float Speed { get; set; }
    public float Direction { get; private set; }

    void Update()
    {
        transform.localPosition += Quaternion.AngleAxis(Direction * 180 / Mathf.PI, new Vector3(0, 0, 1)) * new Vector3(Speed * Time.deltaTime, 0);        
    }

    public void SetDirection(float direction)
    {
        Direction = direction;
        transform.localRotation = Quaternion.Euler(0, 0, Direction * 180 / Mathf.PI - 90);
    }
}
