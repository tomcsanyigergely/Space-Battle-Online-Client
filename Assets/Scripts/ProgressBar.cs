using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject progression;

    private Renderer backgroundRenderer;
    private Renderer progressionRenderer;

    public void Awake()
    {
        backgroundRenderer = background.GetComponent<Renderer>();
        progressionRenderer = progression.GetComponent<Renderer>();
    }

    public void SetProgression(float progressionValue)
    {
        background.transform.localScale = new Vector3(1 - progressionValue, 1, 1);
        background.transform.localPosition = new Vector3(0.5f * progressionValue, 0, 0);

        progression.transform.localScale = new Vector3(progressionValue, 1, 1);
        progression.transform.localPosition = new Vector3(-0.5f + 0.5f * progressionValue, 0, 0);
    }

    public void SetBackgroundColor(Color color)
    {
        backgroundRenderer.material.color = color;
    }

    public void SetProgressionColor(Color color)
    {
        progressionRenderer.material.color = color;
    }    
}
