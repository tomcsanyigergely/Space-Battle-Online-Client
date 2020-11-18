using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdpMessages.ServerClientMessages;

public partial class GameSystem
{
    public void OnConnected()
    {
        if (gameState == GameState.SelectServerMenu)
        {
            selectServerMenu.OnConnected();
        }
        else if (gameState == GameState.CustomServerMenu)
        {
            customServerMenu.OnConnected();
        }
    }

    public void OnDisconnected()
    {
        switch (gameState)
        {
            case GameState.SelectServerMenu:
                selectServerMenu.OnDisconnected();
                break;
            case GameState.CustomServerMenu:
                customServerMenu.OnDisconnected();
                break;
            case GameState.VersionScreen:
                versionScreen.OnDisconnected();
                break;
            case GameState.MainMenu:
                mainMenu.OnDisconnected();
                break;
            case GameState.ReconnectionMenu:
                LostConnectionScreen();
                break;
            case GameState.EnteringMatchScreen:
                LostConnectionScreen();
                break;
            case GameState.EnterUsername:
                LostConnectionScreen();
                break;
            case GameState.SelectPlayersMenu:
                LostConnectionScreen();
                break;
            case GameState.SelectGameModeMenu:
                LostConnectionScreen();
                break;
            case GameState.EnteringQueueScreen:
                LostConnectionScreen();
                break;
            case GameState.QueueMenu:
                LostConnectionScreen();
                break;
            case GameState.EnterRoomMenu:
                LostConnectionScreen();
                break;
            case GameState.CreatingRoomScreen:
                LostConnectionScreen();
                break;
            case GameState.EnteringRoomScreen:
                LostConnectionScreen();
                break;
            case GameState.RoomMenu:
                LostConnectionScreen();
                break;
            case GameState.ClientTokenScreen:
                LostConnectionScreen();
                break;
            case GameState.InGame:
                LostConnectionScreen();
                break;
        }
    }

    public void OnReceived(VersionMessage versionMessage)
    {
        newMatchesAllowed = versionMessage.Fix.Value.NewMatchesAllowed;

        if (gameState == GameState.VersionScreen)
        {
            versionScreen.OnReceived(versionMessage);
        }
    }

    public void OnReceived(NoNewMatchesAllowedMessage noNewMatchesAllowedMessage)
    {
        newMatchesAllowed = false;

        if (gameState != GameState.InGame &&
            gameState != GameState.ClientTokenScreen &&
            gameState != GameState.EnteringMatchScreen &&
            gameState != GameState.ReconnectionMenu &&
            gameState != GameState.VersionScreen)
        {
            MainMenu();
        }
    }

    public void OnReceived(QueueUpdateMessage queueUpdateMessage)
    {
        if (gameState == GameState.EnteringQueueScreen)
        {
            enteringQueueScreen.OnReceived(queueUpdateMessage);
        }
        else if (gameState == GameState.QueueMenu)
        {
            queueMenu.OnReceived(queueUpdateMessage);
        }
    }

    public void OnReceived(QueueLeftMessage queueLeftMessage)
    {
        if (gameState == GameState.EnteringQueueScreen)
        {
            enteringQueueScreen.OnReceived(queueLeftMessage);
        }
        else if (gameState == GameState.QueueMenu)
        {
            queueMenu.OnReceived(queueLeftMessage);
        }
    }

    public void OnReceived(RoomCreatedMessage roomCreatedMessage)
    {
        if (gameState == GameState.CreatingRoomScreen)
        {
            creatingRoomScreen.OnReceived(roomCreatedMessage);
        }
    }

    public void OnReceived(RoomEnteredMessage roomEnteredMessage)
    {
        if (gameState == GameState.EnteringRoomScreen)
        {
            enteringRoomScreen.OnReceived(roomEnteredMessage);
        }
    }

    public void OnReceived(UserEnteredRoomMessage userEnteredRoomMessage)
    {
        if (gameState == GameState.RoomMenu)
        {
            roomMenu.OnReceived(userEnteredRoomMessage);
        }
    }

    public void OnReceived(UserLeftRoomMessage userLeftRoomMessage)
    {
        if (gameState == GameState.RoomMenu)
        {
            roomMenu.OnReceived(userLeftRoomMessage);
        }
    }

    public void OnReceived(RoomDeletedMessage roomDeletedMessage)
    {
        if (gameState == GameState.RoomMenu)
        {
            roomMenu.OnReceived(roomDeletedMessage);
        }
    }

    public void OnReceived(UserSelectedTeamMessage userSelectedTeamMessage)
    {
        if (gameState == GameState.RoomMenu)
        {
            roomMenu.OnReceived(userSelectedTeamMessage);
        }
    }

    public void OnReceived(MatchInitializedMessage matchInitializedMessage)
    {
        if (gameState == GameState.EnteringQueueScreen)
        {
            enteringQueueScreen.OnReceived(matchInitializedMessage);
        }
        else if (gameState == GameState.QueueMenu)
        {
            queueMenu.OnReceived(matchInitializedMessage);
        }
        else if (gameState == GameState.RoomMenu)
        {
            roomMenu.OnReceived(matchInitializedMessage);
        }
    }

    public void OnReceived(InvalidRoomTokenMessage invalidRoomTokenMessage)
    {
        if (gameState == GameState.EnteringRoomScreen)
        {
            enteringRoomScreen.OnReceived(invalidRoomTokenMessage);
        }
    }

    public void OnReceived(InvalidClientTokenMessage invalidClientTokenMessage)
    {
        if (gameState == GameState.EnteringMatchScreen)
        {
            enteringMatchScreen.OnReceived(invalidClientTokenMessage);
        }
    }

    public void OnReceived(InitGameMessage initGameMessage)
    {
        if (gameState == GameState.EnteringMatchScreen)
        {
            enteringMatchScreen.OnReceived(initGameMessage);
        }
        else if (gameState == GameState.InGame)
        {
            inGame.OnReceived(initGameMessage);
        }
    }

    public void OnReceived(MatchStartMessage matchStartMessage)
    {
        if (gameState == GameState.InGame)
        {
            inGame.OnReceived(matchStartMessage);
        }
    }

    public void OnReceived(MatchFinishedMessage matchFinishedMessage)
    {
        if (gameState == GameState.InGame)
        {
            inGame.OnReceived(matchFinishedMessage);
        }
    }

    public void OnReceived(MatchCancelledMessage matchCancelledMessage)
    {
        if (gameState == GameState.InGame)
        {
            inGame.OnReceived(matchCancelledMessage);
        }
    }

    public void OnReceived(PlayerConnectionChangedMessage playerConnectionChangedMessage)
    {
        if (gameState == GameState.InGame)
        {
            inGame.OnReceived(playerConnectionChangedMessage);
        }
    }

    public void OnReceived(PlayerPositionMessage playerPositionMessage)
    {
        if (gameState == GameState.InGame)
        {
            inGame.OnReceived(playerPositionMessage);
        }
    }

    public void OnReceived(SpaceshipRespawnedMessage spaceshipRespawnedMessage)
    {
        if (gameState == GameState.InGame)
        {
            inGame.OnReceived(spaceshipRespawnedMessage);
        }
    }

    public void OnReceived(SpaceshipLifePointsChangedMessage spaceshipLifePointsChangedMessage)
    {
        if (gameState == GameState.InGame)
        {
            inGame.OnReceived(spaceshipLifePointsChangedMessage);
        }
    }

    public void OnReceived(SpaceshipDestroyedMessage spaceshipDestroyedMessage)
    {
        if (gameState == GameState.InGame)
        {
            inGame.OnReceived(spaceshipDestroyedMessage);
        }
    }

    public void OnReceived(SpaceshipHealingChangedMessage spaceshipHealingChangedMessage)
    {
        if (gameState == GameState.InGame)
        {
            inGame.OnReceived(spaceshipHealingChangedMessage);
        }
    }

    public void OnReceived(SpaceshipHitMessage spaceshipHitMessage)
    {
        if (gameState == GameState.InGame)
        {
            inGame.OnReceived(spaceshipHitMessage);
        }
    }

    public void OnReceived(ProjectileShotMessage projectileShotMessage)
    {
        if (gameState == GameState.InGame)
        {
            inGame.OnReceived(projectileShotMessage);
        }
    }

    public void OnReceived(ProjectileDirectionChangedMessage projectileDirectionChangedMessage)
    {
        if (gameState == GameState.InGame)
        {
            inGame.OnReceived(projectileDirectionChangedMessage);
        }
    }

    public void OnReceived(ProjectileDisappearedMessage projectileDisappearedMessage)
    {
        if (gameState == GameState.InGame)
        {
            inGame.OnReceived(projectileDisappearedMessage);
        }
    }

    public void OnReceived(ControlPointTakenMessage controlPointTakenMessage)
    {
        if (gameState == GameState.InGame)
        {
            inGame.OnReceived(controlPointTakenMessage);
        }
    }

    public void OnReceived(ControlPointContestingChangedMessage controlPointContestingChangedMessage)
    {
        if (gameState == GameState.InGame)
        {
            inGame.OnReceived(controlPointContestingChangedMessage);
        }
    }
}
