using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HealthDrawer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;

    public void OnHealthChanged(float healthPoints, float maxHealthPoints)
    {
        float health = healthPoints * 1.0f / maxHealthPoints;
        healthText.text = Convert.ToString(Mathf.CeilToInt(healthPoints));
        healthText.color = new Color(health <= 0.5 ? 1 : 2 - 2 * health, health <= 0.5 ? 2 * health : 1, 0);
    }
}
