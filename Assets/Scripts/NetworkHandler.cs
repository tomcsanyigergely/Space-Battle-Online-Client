using System;
using FlatBuffers;
using UdpMessages.ServerClientMessages;
using UdpMessages.ClientServerMessages;
using UnityEngine;
using TMPro;

public class NetworkHandler : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;

    public FlatBufferBuilder builder;    
    private NetClient netClient;

    private void Awake()
    {
        builder = new FlatBufferBuilder(16384);
        NetClient.Initialize();
        netClient = new NetClient(OnConnected, OnDisconnected, OnReceived);        
    }

    private float timeToListen = 0;

    private void Start()
    {
        timeToListen = 0.05f;
    }    

    private void LateUpdate()
    {
        timeToListen -= Time.deltaTime;

        if (timeToListen <= 0)
        {
            Listen();

            do
            {
                timeToListen += 0.05f;
            } while (timeToListen <= 0);
        }
    }

    public void DelayListen()
    {
        timeToListen = 0.05f;
    }

    public void Connect(string address, UInt16 port) {
        netClient.Connect(address, port);
    }

    public void Disconnect()
    {
        netClient.Disconnect();
    }

    public void Listen() {
        while (netClient.Listen(0) > 0);
    }

    public void Send<T>(Offset<T> message) where T : struct, IFlatbufferObject {
        ClientServerMessage.StartClientServerMessage(builder);

        if (message is Offset<GetVersionMessage>)
        {
            ClientServerMessage.AddContentType(builder, ClientServerMessageContent.GetVersionMessage);
        }
        else if (message is Offset<EnterQueueMessage>)
        {
            ClientServerMessage.AddContentType(builder, ClientServerMessageContent.EnterQueueMessage);
        }
        else if (message is Offset<LeaveQueueMessage>)
        {
            ClientServerMessage.AddContentType(builder, ClientServerMessageContent.LeaveQueueMessage);
        }

        else if (message is Offset<CreateRoomMessage>)
        {
            ClientServerMessage.AddContentType(builder, ClientServerMessageContent.CreateRoomMessage);
        }
        else if (message is Offset<LeaveRoomMessage>)
        {
            ClientServerMessage.AddContentType(builder, ClientServerMessageContent.LeaveRoomMessage);
        }
        else if (message is Offset<EnterRoomMessage>)
        {
            ClientServerMessage.AddContentType(builder, ClientServerMessageContent.EnterRoomMessage);
        }
        else if (message is Offset<StartRoomMessage>)
        {
            ClientServerMessage.AddContentType(builder, ClientServerMessageContent.StartRoomMessage);
        }
        else if (message is Offset<SelectTeamMessage>)
        {
            ClientServerMessage.AddContentType(builder, ClientServerMessageContent.SelectTeamMessage);
        }

        else if (message is Offset<ConnectMessage>)
        {
            ClientServerMessage.AddContentType(builder, ClientServerMessageContent.ConnectMessage);
        }
        else if (message is Offset<ClientReadyMessage>)
        {
            ClientServerMessage.AddContentType(builder, ClientServerMessageContent.ClientReadyMessage);
        }
        else if (message is Offset<ClientInputMessage>)
        {
            ClientServerMessage.AddContentType(builder, ClientServerMessageContent.ClientInputMessage);
        }
        else if (message is Offset<ClientActionMessage>)
        {
            ClientServerMessage.AddContentType(builder, ClientServerMessageContent.ClientActionMessage);
        }
        else
        {
            throw new NotImplementedException("Message type not implemented!");
        }

        ClientServerMessage.AddContent(builder, message.Value);
        var clientServerMessage = ClientServerMessage.EndClientServerMessage(builder);

        builder.Finish(clientServerMessage.Value);

        byte[] buf = builder.SizedByteArray();

        if (message is Offset<ClientInputMessage>)
        {
            netClient.SendUnreliable(buf, Convert.ToUInt16(buf.Length));
        }
        else {
            netClient.SendReliable(buf, Convert.ToUInt16(buf.Length));
        }

        builder.Clear();
    }

    private void OnConnected()
    {
        gameSystem.OnConnected();
    }

    private void OnDisconnected()
    {
        gameSystem.OnDisconnected();
    }

    private void OnReceived(byte[] message, UInt16 size)
    {
        var buf = new ByteBuffer(message);

        var serverClientMessage = ServerClientMessage.GetRootAsServerClientMessage(buf);
        switch (serverClientMessage.ContentType) {
            case ServerClientMessageContent.VersionMessage:
                gameSystem.OnReceived(serverClientMessage.Content<VersionMessage>().Value);
                break;
            case ServerClientMessageContent.NoNewMatchesAllowedMessage:
                gameSystem.OnReceived(serverClientMessage.Content<NoNewMatchesAllowedMessage>().Value);
                break;

            case ServerClientMessageContent.QueueUpdateMessage:
                gameSystem.OnReceived(serverClientMessage.Content<QueueUpdateMessage>().Value);
                break;
            case ServerClientMessageContent.QueueLeftMessage:
                gameSystem.OnReceived(serverClientMessage.Content<QueueLeftMessage>().Value);
                break;

            case ServerClientMessageContent.RoomCreatedMessage:
                gameSystem.OnReceived(serverClientMessage.Content<RoomCreatedMessage>().Value);
                break;
            case ServerClientMessageContent.RoomEnteredMessage:
                gameSystem.OnReceived(serverClientMessage.Content<RoomEnteredMessage>().Value);
                break;
            case ServerClientMessageContent.UserEnteredRoomMessage:
                gameSystem.OnReceived(serverClientMessage.Content<UserEnteredRoomMessage>().Value);
                break;
            case ServerClientMessageContent.UserLeftRoomMessage:
                gameSystem.OnReceived(serverClientMessage.Content<UserLeftRoomMessage>().Value);
                break;
            case ServerClientMessageContent.RoomDeletedMessage:
                gameSystem.OnReceived(serverClientMessage.Content<RoomDeletedMessage>().Value);
                break;
            case ServerClientMessageContent.UserSelectedTeamMessage:
                gameSystem.OnReceived(serverClientMessage.Content<UserSelectedTeamMessage>().Value);
                break;

            case ServerClientMessageContent.InvalidRoomTokenMessage:
                gameSystem.OnReceived(serverClientMessage.Content<InvalidRoomTokenMessage>().Value);
                break;
            case ServerClientMessageContent.InvalidClientTokenMessage:
                gameSystem.OnReceived(serverClientMessage.Content<InvalidClientTokenMessage>().Value);
                break;

            case ServerClientMessageContent.MatchInitializedMessage:
                gameSystem.OnReceived(serverClientMessage.Content<MatchInitializedMessage>().Value);
                break;
            case ServerClientMessageContent.InitGameMessage:
                gameSystem.OnReceived(serverClientMessage.Content<InitGameMessage>().Value);
                break;
            case ServerClientMessageContent.MatchStartMessage:
                gameSystem.OnReceived(serverClientMessage.Content<MatchStartMessage>().Value);
                break;
            case ServerClientMessageContent.MatchFinishedMessage:
                gameSystem.OnReceived(serverClientMessage.Content<MatchFinishedMessage>().Value);
                break;
            case ServerClientMessageContent.MatchCancelledMessage:
                gameSystem.OnReceived(serverClientMessage.Content<MatchCancelledMessage>().Value);
                break;
            case ServerClientMessageContent.PlayerConnectionChangedMessage:
                gameSystem.OnReceived(serverClientMessage.Content<PlayerConnectionChangedMessage>().Value);
                break;
            case ServerClientMessageContent.PlayerPositionMessage:
                gameSystem.OnReceived(serverClientMessage.Content<PlayerPositionMessage>().Value);
                break;
            case ServerClientMessageContent.SpaceshipRespawnedMessage:
                gameSystem.OnReceived(serverClientMessage.Content<SpaceshipRespawnedMessage>().Value);
                break;
            case ServerClientMessageContent.SpaceshipLifePointsChangedMessage:
                gameSystem.OnReceived(serverClientMessage.Content<SpaceshipLifePointsChangedMessage>().Value);
                break;
            case ServerClientMessageContent.SpaceshipDestroyedMessage:
                gameSystem.OnReceived(serverClientMessage.Content<SpaceshipDestroyedMessage>().Value);
                break;
            case ServerClientMessageContent.SpaceshipHealingChangedMessage:
                gameSystem.OnReceived(serverClientMessage.Content<SpaceshipHealingChangedMessage>().Value);
                break;
            case ServerClientMessageContent.SpaceshipHitMessage:
                gameSystem.OnReceived(serverClientMessage.Content<SpaceshipHitMessage>().Value);
                break;
            case ServerClientMessageContent.ProjectileShotMessage:
                gameSystem.OnReceived(serverClientMessage.Content<ProjectileShotMessage>().Value);
                break;
            case ServerClientMessageContent.ProjectileDirectionChangedMessage:
                gameSystem.OnReceived(serverClientMessage.Content<ProjectileDirectionChangedMessage>().Value);
                break;
            case ServerClientMessageContent.ProjectileDisappearedMessage:
                gameSystem.OnReceived(serverClientMessage.Content<ProjectileDisappearedMessage>().Value);
                break;
            case ServerClientMessageContent.ControlPointTakenMessage:
                gameSystem.OnReceived(serverClientMessage.Content<ControlPointTakenMessage>().Value);
                break;
            case ServerClientMessageContent.ControlPointContestingChangedMessage:
                gameSystem.OnReceived(serverClientMessage.Content<ControlPointContestingChangedMessage>().Value);
                break;
        }
    }
}
