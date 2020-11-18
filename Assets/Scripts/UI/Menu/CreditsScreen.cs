using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScreen : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;

    [SerializeField] private Canvas creditsScreen;

    [SerializeField] private Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(() => { gameSystem.SelectServerMenu(); });

        UIUtilities.Hide(creditsScreen);
    }

    public void Enter()
    {
        UIUtilities.Show(creditsScreen);
    }

    public void Exit()
    {
        UIUtilities.Hide(creditsScreen);
    }
}
