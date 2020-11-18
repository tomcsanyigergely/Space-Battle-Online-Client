using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pulsate : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float pulseSpeed;

    private Color color;

    private void Start()
    {
        color = image.color;
    }

    private void Update()
    {
        image.color = new Color(color.r, color.g, color.b, 0.5f + 0.5f * Mathf.Sin(pulseSpeed * Time.time));
    }
}
