using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdpMessages.ServerClientMessages;
using UInt8 = System.Byte;
using UnityEngine.UI;

public partial class GameSystem : MonoBehaviour
{
    private enum GameState {
        SelectServerMenu,
        CustomServerMenu,
        CreditsScreen,
        VersionScreen,
        MainMenu,
        ReconnectionMenu,
        EnterUsername,
        SelectPlayersMenu,
        SelectGameModeMenu,
        EnteringQueueScreen,
        QueueMenu,
        EnterRoomMenu,
        CreatingRoomScreen,
        EnteringRoomScreen,
        RoomMenu,
        ClientTokenScreen,
        EnteringMatchScreen,
        InGame,
        LostConnectionScreen
    }

    public enum Players {
        Random, Invited
    }

    [SerializeField] private NetworkHandler networkHandler;

    [SerializeField] private Canvas gui;

    [SerializeField] private SelectServerMenu selectServerMenu;
    [SerializeField] private CustomServerMenu customServerMenu;
    [SerializeField] private CreditsScreen creditsScreen;
    [SerializeField] private VersionScreen versionScreen;
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private ReconnectionMenu reconnectionMenu;
    [SerializeField] private EnteringMatchScreen enteringMatchScreen;
    [SerializeField] private EnterUsername enterUsername;
    [SerializeField] private SelectPlayersMenu selectPlayersMenu;
    [SerializeField] private SelectGameModeMenu selectGameModeMenu;
    [SerializeField] private EnteringQueueScreen enteringQueueScreen;
    [SerializeField] private QueueMenu queueMenu;
    [SerializeField] private EnterRoomMenu enterRoomMenu;
    [SerializeField] private CreatingRoomScreen creatingRoomScreen;
    [SerializeField] private EnteringRoomScreen enteringRoomScreen;
    [SerializeField] private RoomMenu roomMenu;
    [SerializeField] private ClientTokenScreen clientTokenScreen;
    [SerializeField] private GameManager inGame;
    [SerializeField] private LostConnectionScreen lostConnectionScreen;

    [SerializeField] private AudioSource menuMusic;
    [SerializeField] private AudioSource buttonClickAudio;

    private GameState gameState;
    private bool newMatchesAllowed = false;

    private void Start()
    {
        foreach (Button button in gui.GetComponentsInChildren<Button>())
        {
            button.onClick.AddListener(() => { buttonClickAudio.Play(); });
        }
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.VersionScreen:
                versionScreen.MenuUpdate();
                break;
            case GameState.EnteringMatchScreen:
                enteringMatchScreen.MenuUpdate();
                break;
            case GameState.CreatingRoomScreen:
                creatingRoomScreen.MenuUpdate();
                break;
            case GameState.EnteringRoomScreen:
                enteringRoomScreen.MenuUpdate();
                break;
        }
    }

    private void ExitPrevious()
    {
        switch (gameState)
        {
            case GameState.SelectServerMenu:
                selectServerMenu.Exit();
                break;
            case GameState.CustomServerMenu:
                customServerMenu.Exit();
                break;
            case GameState.CreditsScreen:
                creditsScreen.Exit();
                break;
            case GameState.VersionScreen:
                versionScreen.Exit();
                break;
            case GameState.MainMenu:
                mainMenu.Exit();
                break;
            case GameState.ReconnectionMenu:
                reconnectionMenu.Exit();
                break;
            case GameState.EnteringMatchScreen:
                enteringMatchScreen.Exit();
                break;
            case GameState.EnterUsername:
                enterUsername.Exit();
                break;
            case GameState.SelectPlayersMenu:
                selectPlayersMenu.Exit();
                break;
            case GameState.SelectGameModeMenu:
                selectGameModeMenu.Exit();
                break;
            case GameState.EnteringQueueScreen:
                enteringQueueScreen.Exit();
                break;
            case GameState.QueueMenu:
                queueMenu.Exit();
                break;
            case GameState.EnterRoomMenu:
                enterRoomMenu.Exit();
                break;
            case GameState.CreatingRoomScreen:
                creatingRoomScreen.Exit();
                break;
            case GameState.EnteringRoomScreen:
                enteringRoomScreen.Exit();
                break;
            case GameState.RoomMenu:
                roomMenu.Exit();
                break;
            case GameState.ClientTokenScreen:
                clientTokenScreen.Exit();
                break;
            case GameState.LostConnectionScreen:
                lostConnectionScreen.Exit();
                break;
            case GameState.InGame:
                inGame.Exit();
                menuMusic.Play();
                break;
        }
    }

    public void SelectServerMenu()
    {
        ExitPrevious();
        gameState = GameState.SelectServerMenu;
        selectServerMenu.Enter();
    }

    public void CustomServerMenu()
    {
        ExitPrevious();
        gameState = GameState.CustomServerMenu;
        customServerMenu.Enter();
    }

    public void CreditsScreen()
    {
        ExitPrevious();
        gameState = GameState.CreditsScreen;
        creditsScreen.Enter();
    }

    public void VersionScreen()
    {
        ExitPrevious();
        gameState = GameState.VersionScreen;
        versionScreen.Enter();
    }

    public void MainMenu()
    {
        ExitPrevious();
        gameState = GameState.MainMenu;
        mainMenu.Enter(newMatchesAllowed);
    }

    public void ReconnectionMenu()
    {
        ExitPrevious();
        gameState = GameState.ReconnectionMenu;
        reconnectionMenu.Enter();
    }

    public void EnteringMatchScreen(UInt64 clientToken)
    {
        ExitPrevious();
        gameState = GameState.EnteringMatchScreen;
        enteringMatchScreen.Enter(clientToken);
    }

    public void EnterUsername()
    {
        ExitPrevious();
        gameState = GameState.EnterUsername;
        enterUsername.Enter();
    }

    public void SelectPlayersMenu(string username)
    {
        ExitPrevious();
        gameState = GameState.SelectPlayersMenu;
        selectPlayersMenu.Enter(username);
    }

    public void SelectGameModeMenu(string username, Players players)
    {
        ExitPrevious();
        gameState = GameState.SelectGameModeMenu;
        selectGameModeMenu.Enter(username, players);
    }

    public void EnteringQueueScreen(string username, UInt8 teamSize)
    {
        ExitPrevious();
        gameState = GameState.EnteringQueueScreen;
        enteringQueueScreen.Enter(username, teamSize);
    }

    public void QueueMenu(string username, UInt8 teamSize, bool leaving, QueueUpdateMessage queueUpdateMessage)
    {
        ExitPrevious();
        gameState = GameState.QueueMenu;
        queueMenu.Enter(username, teamSize, leaving, queueUpdateMessage);
    }

    public void EnterRoomMenu(string username)
    {
        ExitPrevious();
        gameState = GameState.EnterRoomMenu;
        enterRoomMenu.Enter(username);
    }

    public void CreatingRoomScreen(string username, UInt8 teamSize)
    {
        ExitPrevious();
        gameState = GameState.CreatingRoomScreen;
        creatingRoomScreen.Enter(username, teamSize);
    }

    public void EnteringRoomScreen(string username, UInt64 roomToken)
    {
        ExitPrevious();
        gameState = GameState.EnteringRoomScreen;
        enteringRoomScreen.Enter(username, roomToken);
    }

    public void RoomMenu(string username, RoomCreatedMessage roomCreatedMessage)
    {
        ExitPrevious();
        gameState = GameState.RoomMenu;
        roomMenu.Enter(username, roomCreatedMessage);
    }

    public void RoomMenu(string username, RoomEnteredMessage roomEnteredMessage)
    {
        ExitPrevious();
        gameState = GameState.RoomMenu;
        roomMenu.Enter(username, roomEnteredMessage);
    }

    public void ClientTokenScreen(MatchInitializedMessage matchInitializedMessage)
    {
        ExitPrevious();
        gameState = GameState.ClientTokenScreen;
        clientTokenScreen.Enter(matchInitializedMessage);
    }

    public void InGame(InitGameMessage initGameMessage)
    {
        ExitPrevious();
        gameState = GameState.InGame;
        menuMusic.Stop();
        inGame.Enter(initGameMessage);
    }

    public void LostConnectionScreen()
    {
        ExitPrevious();
        gameState = GameState.LostConnectionScreen;
        lostConnectionScreen.Enter();
    }

    public void LeaveInGame(string username)
    {
        if (newMatchesAllowed)
        {
            SelectPlayersMenu(username);
        }
        else
        {
            MainMenu();
        }
    }
}
