using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityAction<float, float> onHealthChanged;
    public UnityAction<bool> onHealingChanged;

    public float HealthPoints { get; private set; }
    public float MaxHealthPoints { get; private set; }
    public float HealingSpeed { get; private set; }

    public void Init(float maxHealthPoints)
    {
        MaxHealthPoints = maxHealthPoints;
    }

    private void Update()
    {
        float previousHealthPoints = HealthPoints;
        if (HealingSpeed != 0 && HealthPoints < MaxHealthPoints)
        {
            HealthPoints += Time.deltaTime * HealingSpeed;
            if (HealthPoints > MaxHealthPoints)
            {
                HealthPoints = MaxHealthPoints;
            }
            onHealthChanged(HealthPoints, MaxHealthPoints);
        }
    }

    public void SetHealthPoints(float healthPoints)
    {
        HealthPoints = healthPoints;
        onHealthChanged(HealthPoints, MaxHealthPoints);
    }

    public void SetHealingSpeed(float healingSpeed)
    {
        HealingSpeed = healingSpeed;
    }
}
