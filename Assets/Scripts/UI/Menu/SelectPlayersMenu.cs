using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayersMenu : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;

    [SerializeField] private Canvas selectPlayerMenu;

    [SerializeField] private Button randomPlayersButton;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;
    [SerializeField] private Button cancelButton;

    private string username;

    private void Start()
    {
        randomPlayersButton.onClick.AddListener(() => {
            gameSystem.SelectGameModeMenu(username, GameSystem.Players.Random);
        });

        createRoomButton.onClick.AddListener(() => {
            gameSystem.SelectGameModeMenu(username, GameSystem.Players.Invited);
        });

        joinRoomButton.onClick.AddListener(() => {
            gameSystem.EnterRoomMenu(username);
        });

        cancelButton.onClick.AddListener(() => {
            gameSystem.EnterUsername();
        });

        UIUtilities.Hide(selectPlayerMenu);
    }

    public void Enter(string username)
    {
        this.username = username;
        UIUtilities.Show(selectPlayerMenu);
    }

    public void Exit()
    {
        UIUtilities.Hide(selectPlayerMenu);
    }
}
