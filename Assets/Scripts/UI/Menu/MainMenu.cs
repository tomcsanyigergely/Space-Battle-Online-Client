using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;
    [SerializeField] private NetworkHandler networkHandler;

    [SerializeField] private Canvas mainMenu;

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button rejoinButton;
    [SerializeField] private Button disconnectButton;

    [SerializeField] private TextMeshProUGUI noNewMatchesAllowedText;

    private bool disconnecting;

    private void Start()
    {
        newGameButton.onClick.AddListener(() => {
            gameSystem.EnterUsername();
        });

        rejoinButton.onClick.AddListener(() => {
            gameSystem.ReconnectionMenu();
        });

        disconnectButton.onClick.AddListener(() => {
            disconnecting = true;
            newGameButton.interactable = false;
            rejoinButton.interactable = false;
            networkHandler.Disconnect();
        });

        UIUtilities.Hide(mainMenu);
    }

    public void Enter(bool newMatchesAllowed)
    {
        disconnecting = false;
        noNewMatchesAllowedText.gameObject.SetActive(!newMatchesAllowed);
        newGameButton.gameObject.SetActive(newMatchesAllowed);
        newGameButton.interactable = true;
        rejoinButton.interactable = true;

        UIUtilities.Show(mainMenu);
    }

    public void Exit()
    {
        UIUtilities.Hide(mainMenu);
    }

    public void OnDisconnected()
    {
        if (disconnecting)
        {
            gameSystem.SelectServerMenu();
        }
        else
        {
            gameSystem.LostConnectionScreen();
        }
    }
}
