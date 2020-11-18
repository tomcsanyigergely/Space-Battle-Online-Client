using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UInt8 = System.Byte;

public class CaptureBar : MonoBehaviour
{
    [SerializeField] private GameObject captureBar;

    private ProgressBar progressBarComponent;

    private Color[] playerColors;

    private void Awake()
    {
        GetComponent<ControlPoint>().onProgressionChanged += OnProgressionChanged;
        GetComponent<ControlPoint>().onProgressedPlayerChanged += OnProgressedPlayerChanged;
    }

    public void Init(Vector3 captureBarPosition, Vector3 captureBarScale, Color[] playerColors)
    {
        progressBarComponent = captureBar.GetComponent<ProgressBar>();
        progressBarComponent.SetBackgroundColor(Color.white);

        captureBar.transform.localPosition = captureBarPosition;
        captureBar.transform.localScale = captureBarScale;

        this.playerColors = playerColors;
    }

    private void OnProgressedPlayerChanged(UInt8 progressedPlayer)
    {
        progressBarComponent.SetProgressionColor(playerColors[progressedPlayer]);
    }

    private void OnProgressionChanged(float progression, float maxProgression)
    {
        progressBarComponent.SetProgression(progression / maxProgression);
    }
}
