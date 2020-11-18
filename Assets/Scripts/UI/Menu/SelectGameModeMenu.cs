using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UdpMessages.ClientServerMessages;
using UdpMessages.ServerClientMessages;
using UInt8 = System.Byte;

public class SelectGameModeMenu : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;

    [SerializeField] private Canvas selectGameModeMenu;

    [SerializeField] private Button oneVersusOneButton;
    [SerializeField] private Button twoVersusTwoButton;
    [SerializeField] private Button threeVersusThreeButton;
    [SerializeField] private Button cancelButton;

    private string username;
    private GameSystem.Players players;

    private void Start()
    {
        oneVersusOneButton.onClick.AddListener(() => {
            SelectTeamSize(1);
        });

        twoVersusTwoButton.onClick.AddListener(() => {
            SelectTeamSize(2);
        });

        threeVersusThreeButton.onClick.AddListener(() => {
            SelectTeamSize(3);
        });

        cancelButton.onClick.AddListener(() => {
            gameSystem.SelectPlayersMenu(username);
        });

        UIUtilities.Hide(selectGameModeMenu);
    }

    public void Enter(string username, GameSystem.Players players)
    {
        this.username = username;
        this.players = players;
        UIUtilities.Show(selectGameModeMenu);
    }

    public void Exit()
    {
        UIUtilities.Hide(selectGameModeMenu);
    }

    private void SelectTeamSize(UInt8 teamSize)
    {
        switch (players)
        {
            case GameSystem.Players.Random:
                gameSystem.EnteringQueueScreen(username, teamSize);
                break;

            case GameSystem.Players.Invited:
                gameSystem.CreatingRoomScreen(username, teamSize);
                break;
        }
    }
}
