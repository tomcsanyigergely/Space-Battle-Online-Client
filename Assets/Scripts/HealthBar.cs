using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;

    private ProgressBar progressBarComponent;

    private void Awake()
    {
        progressBarComponent = healthBar.GetComponent<ProgressBar>();

        Health healthComponent = GetComponent<Health>();
        if (healthComponent != null) {
            healthComponent.onHealthChanged += OnHealthChanged;
        }
    }

    public void Start()
    {        
        progressBarComponent.SetBackgroundColor(Color.black);
        progressBarComponent.SetProgressionColor(Color.green);
    }

    public void Init(Vector3 healthBarPosition, Vector3 healthBarScale)
    {
        healthBar.transform.localPosition = healthBarPosition;
        healthBar.transform.localScale = healthBarScale;
    }

    public void OnHealthChanged(float healthPoints, float maxHealthPoints)
    {
        float health = Mathf.CeilToInt(Mathf.Max(0, healthPoints)) * 1.0f / maxHealthPoints;
        progressBarComponent.SetProgression(health);
        progressBarComponent.SetProgressionColor(new Color(health <= 0.5 ? 1 : 2-2*health, health <= 0.5 ? 2*health : 1, 0));
    }
}
