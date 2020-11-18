using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReinstallTimeDrawer : MonoBehaviour
{
    [SerializeField] private GameObject reinstallMarker;
    [SerializeField] private TextMeshProUGUI reinstallTimeText;

    private void Awake()
    {
        GetComponent<ReinstallTime>().onRemainingTimeChanged += OnRemainingTimeChanged;
    }

    private void OnRemainingTimeChanged(float remainingTime)
    {
        if (remainingTime > 0)
        {
            reinstallTimeText.text = remainingTime.ToString("0.0");
            reinstallMarker.SetActive(true);
        }
        else
        {
            reinstallMarker.SetActive(false);
        }
    }
}
