using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityAction onDamaged;

    public void Damage()
    {
        onDamaged?.Invoke();
    }
}
