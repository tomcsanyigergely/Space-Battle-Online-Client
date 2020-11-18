using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControlProgression : MonoBehaviour
{
    public UnityAction<float, float> onControlProgressionChanged;

    private float maxControlProgression;
    private float controlProgressionSpeed;

    public float controlProgression { get; private set; }
    private bool enableProgress = false;

    public void Init(float maxControlProgression, float controlProgressionSpeed)
    {
        this.maxControlProgression = maxControlProgression;
        this.controlProgressionSpeed = controlProgressionSpeed;
    }

    void Update()
    {
        if (enableProgress && controlProgression < maxControlProgression)
        {
            controlProgression += Time.deltaTime * controlProgressionSpeed;
            if (controlProgression > maxControlProgression)
            {
                controlProgression = maxControlProgression;
            }

            onControlProgressionChanged(controlProgression, maxControlProgression);
        }
    }

    public void EnableProgress(bool enableProgress)
    {
        this.enableProgress = enableProgress;
    }

    public void SetControlProgression(float controlProgression)
    {
        this.controlProgression = controlProgression;
        onControlProgressionChanged(controlProgression, maxControlProgression);
    }    
}
