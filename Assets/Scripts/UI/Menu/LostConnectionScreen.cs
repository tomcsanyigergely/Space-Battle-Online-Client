using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LostConnectionScreen : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;

    [SerializeField] private Canvas lostConnectionScreen;

    [SerializeField] private Button okButton;

    private void Start()
    {
        okButton.onClick.AddListener(() => { gameSystem.SelectServerMenu(); });

        UIUtilities.Hide(lostConnectionScreen);
    }

    public void Enter()
    {
        UIUtilities.Show(lostConnectionScreen);
    }

    public void Exit()
    {
        UIUtilities.Hide(lostConnectionScreen);
    }
}
