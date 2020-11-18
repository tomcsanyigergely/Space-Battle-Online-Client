using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ControlProgressionDrawer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI controlProgressionText;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private Color progressionColor;

    private ProgressBar progressBarComponent;

    private void Awake()
    {
        progressBarComponent = progressBar.GetComponent<ProgressBar>();

        GetComponent<ControlProgression>().onControlProgressionChanged += OnControlProgressionChanged;
    }

    private void Start()
    {        
        progressBarComponent.SetBackgroundColor(new Color(0.3f, 0.3f, 0.3f));
        progressBarComponent.SetProgressionColor(progressionColor);
        progressBarComponent.SetProgression(0.75f);
    }

    private void OnControlProgressionChanged(float controlProgression, float maxControlProgression)
    {
        controlProgressionText.text = (Mathf.Floor(controlProgression * 100) / 100f).ToString("0.00");
        progressBarComponent.SetProgression(controlProgression / maxControlProgression);
    }
}
