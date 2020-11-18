using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ShowFrameRate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;

    private int frameCount = 0;
    private float dt = 0;
    private float fps = 0;
    private const float updateRate = 4;
    
    void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0 / updateRate)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;

            fpsText.text = "FPS: " + Convert.ToString(fps);
        }
    }
}
