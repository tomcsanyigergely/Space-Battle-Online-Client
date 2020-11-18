using System;
using System.Collections.Generic;
using TMPro;
using UdpMessages.ClientServerMessages;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UdpMessages.ServerClientMessages;
using UInt8 = System.Byte;

public partial class GameManager : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;
    [SerializeField] private NetworkHandler networkHandler;

    [SerializeField] private GameObject gameArea;    

    private enum ClientState { WaitingForOpponent, Countdown, Running, Finished }
    private ClientState clientState;

    private UInt64 clientToken;

    // -------------------------------------------------

    [SerializeField] private TextMeshProUGUI playerPosMsgCountText;

    [SerializeField] private AudioSource explosionAudio;
    [SerializeField] private AudioSource hurtAudio;
    [SerializeField] private AudioSource normalProjectileAudio;
    [SerializeField] private AudioSource playerAttackedAudio;
    [SerializeField] private AudioSource playerAttackingAudio;
    [SerializeField] private AudioSource playerTakenCPAudio;
    [SerializeField] private AudioSource playerLostCPAudio;
    [SerializeField] private AudioSource enemyTakenNeutralCPAudio;
    [SerializeField] private AudioSource enemyHitAudio;
    [SerializeField] private AudioSource warningAudio;
    [SerializeField] private AudioSource countdownAudio;

    [SerializeField] private AudioSource ingameMusic;

    // -------------------------------------------------
        
    [SerializeField] private Canvas inGameCanvas;
    [SerializeField] private Canvas floatingUsernameCanvas;
    
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button interpolateButton;
    [SerializeField] private Canvas inGameHUD;

    [SerializeField] private GameObject playerHealthBar;

    [SerializeField] private TextMeshProUGUI normalProjectileAmmoText;
    [SerializeField] private TextMeshProUGUI bouncingProjectileAmmoText;

    [SerializeField] private TextMeshProUGUI playerControlledPointsText;
    [SerializeField] private TextMeshProUGUI enemyControlledPointsText;

    [SerializeField] private GameObject playerControlledPointsBackground;
    [SerializeField] private GameObject enemyControlledPointsBackground;

    [SerializeField] private GameObject playerControlProgression;
    [SerializeField] private GameObject enemyControlProgression;

    [SerializeField] private GameObject playerProgressionData;
    [SerializeField] private GameObject enemyProgressionData;

    [SerializeField] private GameObject enemyNumberBackgroundHighlighted;
    [SerializeField] private GameObject enemyProgressionHighlighted;

    [SerializeField] private TextMeshProUGUI[] allUsernameTexts;
    [SerializeField] private Image[] allDisconnectedIcons;
    [SerializeField] private Image[] allConnectingIcons;

    [SerializeField] private TextMeshProUGUI timeToDrawText;

    [SerializeField] private TextMeshProUGUI[] floatingUsernameTexts;
    // ------------------------------------------------

    [SerializeField] private GameObject spaceshipPrefab;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject controlPointPrefab;
    [SerializeField] private GameObject respawnPointPrefab;

    [SerializeField] private GameObject flashParticlesPrefab;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera backgroundCamera;
    [SerializeField] private Camera uiCamera;

    [SerializeField] private Material asteroidMaterial;

    // -------------------------------------------------

    private float gameScale = Config.GAME_DEFAULT_SCALE;

    private TextMeshProUGUI[] playerUsernameTexts;
    private Image[] playerDisconnectedIcons;
    private Image[] playerConnectingIcons;
    private GameObject[] progressionDatas = new GameObject[2];
    private TextMeshProUGUI[] controlledPointsTexts = new TextMeshProUGUI[2];
    private GameObject[] controlledPointsBackgrounds = new GameObject[2];
    private GameObject[] controlProgressions = new GameObject[2];

    private List<GameObject> spaceshipShadows = new List<GameObject>();
    private List<GameObject> spaceships = new List<GameObject>();
    private List<GameObject> respawnPoints = new List<GameObject>();
    private List<GameObject> projectiles = new List<GameObject>();
    private List<GameObject> obstacles = new List<GameObject>();
    private List<GameObject> controlPoints = new List<GameObject>();

    private LinkedList<Tuple<float, Action>> actions = new LinkedList<Tuple<float, Action>>();

    private ParticleSystem[] flashParticles = new ParticleSystem[50];
    private int nextFlashParticleIndex = 0;
        
    private Color[] playerColors = new Color[2];
    private Color[] controlPointColors = new Color[2];
    private Color[] respawnPointColors = new Color[2];

    private float inputMultiplier;

    private UInt8 playerNumber;
    private UInt8 teamSize;

    private UInt8 spaceshipRespawnTime;
    private float spaceshipMaxLifePoints;
    private float spaceshipRadius;

    private float normalProjectileRadius;
    private float normalProjectileSpeed;
    private UInt8 normalProjectileClipSize;
    private float normalProjectileReloadDelay;
    private float normalProjectileReloadSpeed;

    private float bouncingProjectileRadius;
    private float bouncingProjectileSpeed;
    private UInt8 bouncingProjectileClipSize;
    private float bouncingProjectileAutoReloadSpeed;

    private float controlPointRadius;
    private float controlPointTakingProgression;
    private float controlPointProgressionSpeed;
    private float controlPointTimeBeforeCooldown;
    private float controlPointCooldownSpeed;
    private float maxControlProgression;
    private float controlProgressionSpeed;
    private float controlFinalizationDuration;
    private float timeToDraw;

    private Color teammateAliveUsernameColor = new Color(0f, 255.0f / 255.0f, 249.0f / 255.0f);
    private Color teammateDeadUsernameColor = new Color(0f, 145.0f / 255.0f, 141.0f / 255.0f);

    private Color opponentAliveUsernameColor = new Color(255.0f / 255.0f, 0f, 0f);
    private Color opponentDeadUsernameColor = new Color(157.0f / 255.0f, 0f, 0f);

    private string username;

    private float timeToSendVelocity;

    public void Start()
    {
        Application.targetFrameRate = 60;

        gameArea.gameObject.SetActive(false);

        continueButton.onClick.AddListener(() => {
            gameSystem.LeaveInGame(username);
        });

        interpolateButton.onClick.AddListener(() => {
            Interpolate.interpolate = !Interpolate.interpolate;

            if (Interpolate.interpolate)
            {
                interpolateButton.GetComponentInChildren<TextMeshProUGUI>().text = "Interpolation ON";
            }
            else
            {
                foreach (var action in actions)
                {
                    action.Item2();
                }
                actions.Clear();

                interpolateButton.GetComponentInChildren<TextMeshProUGUI>().text = "Interpolation OFF";
            }
        });

        gameArea.transform.localScale = new Vector3(gameScale, gameScale, 1);        

        UIUtilities.Hide(inGameCanvas);
    }    

    public void Enter(InitGameMessage initGameMessage) {
        timeToSendVelocity = 0;
        gameArea.gameObject.SetActive(true);

        playerPositionMessageCount = 0;
        timeToShowPlayerPositionMessageCount = 0;
        playerPosMsgCountText.gameObject.SetActive(false);

        username = initGameMessage.Spaceships(initGameMessage.Fix.Value.PlayerNumber).Value.Username;

        interpolateButton.gameObject.SetActive(false);
        interpolateButton.GetComponentInChildren<TextMeshProUGUI>().text = "Interpolation ON";
        Interpolate.interpolate = true;

        continueButton.gameObject.SetActive(false);

        TransitionToWarningState(WarningState.NoWarning);

        foreach (var usernameText in allUsernameTexts)
        {
            usernameText.text = "";
        }

        foreach (var floatingUsernameText in floatingUsernameTexts)
        {
            floatingUsernameText.text = "";
        }

        foreach (var disconnectedIcon in allDisconnectedIcons)
        {
            disconnectedIcon.gameObject.SetActive(false);
        }

        foreach (var connectingIcon in allConnectingIcons)
        {
            connectingIcon.gameObject.SetActive(false);
        }

        for (int i = 0; i < flashParticles.Length; i++)
        {
            GameObject flashParticle = Instantiate(flashParticlesPrefab);
            flashParticle.transform.parent = gameArea.transform;
            flashParticle.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            flashParticles[i] = flashParticle.GetComponent<ParticleSystem>();
        }

        clientState = ClientState.WaitingForOpponent;

        InitializeGameState(initGameMessage);

        UIUtilities.Show(inGameCanvas);
    }

    public void Exit()
    {
        ingameMusic.Stop();

        gameArea.gameObject.SetActive(false);

        actions.Clear();
        clientState = ClientState.WaitingForOpponent;

        foreach (var spaceshipShasow in spaceshipShadows) { Destroy(spaceshipShasow); }
        spaceshipShadows.Clear();

        foreach (var spaceship in spaceships) { Destroy(spaceship); }
        spaceships.Clear();

        foreach (var respawnPoint in respawnPoints) { Destroy(respawnPoint); }
        respawnPoints.Clear();

        foreach (var projectile in projectiles) { Destroy(projectile); }
        projectiles.Clear();

        foreach (var obstacle in obstacles) { Destroy(obstacle); }
        obstacles.Clear();

        foreach (var controlPoint in controlPoints) { Destroy(controlPoint); }
        controlPoints.Clear();

        foreach (var flashParticle in flashParticles)
        {
            Destroy(flashParticle);
        }

        UIUtilities.Hide(inGameCanvas);
        backgroundCamera.GetComponent<Spin>().enabled = true;
    }

    private void LateUpdate()
    {
        if (clientState == ClientState.Countdown || clientState == ClientState.Running)
        {
            mainCamera.transform.position = new Vector3(spaceships[playerNumber].transform.position.x, spaceships[playerNumber].transform.position.y, -25);
            backgroundCamera.transform.localEulerAngles = new Vector3(-5 * spaceships[playerNumber].transform.localPosition.y, 5 * spaceships[playerNumber].transform.localPosition.x, 0) * inputMultiplier;            
        }

        for (UInt8 playerIndex = 0; playerIndex < spaceships.Count; playerIndex++)
        {
            Vector2 screenPos = mainCamera.WorldToScreenPoint(spaceships[playerIndex].transform.Find("Empty").Find("Health Bar").position);

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(floatingUsernameCanvas.GetComponent<RectTransform>(), screenPos, uiCamera, out localPoint);

            Vector2 textPosition = localPoint + new Vector2(0, 25 / Config.GAME_MAX_SCALE * gameScale);

            floatingUsernameTexts[playerIndex].transform.localPosition = new Vector3(textPosition.x, textPosition.y, floatingUsernameTexts[playerIndex].transform.localPosition.z);
            floatingUsernameTexts[playerIndex].transform.localScale = new Vector3(1, 1, 1) * (gameScale / Config.GAME_MAX_SCALE);
        }
    }

    private int playerPositionMessageCount;
    private float timeToShowPlayerPositionMessageCount;

    public void Update()
    {
        while (actions.Count > 0 && actions.First.Value.Item1 <= Time.time)
        {
            actions.First.Value.Item2();
            actions.RemoveFirst();
        }

        if (clientState == ClientState.Countdown || clientState == ClientState.Running)
        {
            if (Input.mouseScrollDelta.y > 0 && gameScale < Config.GAME_MAX_SCALE)
            {
                gameScale += Input.mouseScrollDelta.y;
                if (gameScale > Config.GAME_MAX_SCALE)
                {
                    gameScale = Config.GAME_MAX_SCALE;
                }
                gameArea.transform.localScale = new Vector3(gameScale, gameScale, 1);
            }
            else if (Input.mouseScrollDelta.y < 0 && gameScale > Config.GAME_MIN_SCALE)
            {
                gameScale += Input.mouseScrollDelta.y;
                if (gameScale < Config.GAME_MIN_SCALE)
                {
                    gameScale = Config.GAME_MIN_SCALE;
                }
                gameArea.transform.localScale = new Vector3(gameScale, gameScale, 1);
            }
        }

        if (clientState == ClientState.Running) {
            /*if (Input.GetKeyDown(KeyCode.I))
            {
                Interpolate.interpolate = !Interpolate.interpolate;

                if (Interpolate.interpolate)
                {
                    //interpolateButton.GetComponentInChildren<TextMeshProUGUI>().text = "Interpolation ON";
                }
                else
                {
                    foreach (var action in actions)
                    {
                        action.Item2();
                    }
                    actions.Clear();

                    //interpolateButton.GetComponentInChildren<TextMeshProUGUI>().text = "Interpolation OFF";
                }
            }*/

            if (Input.GetKeyDown(KeyCode.P))
            {
                playerPosMsgCountText.gameObject.SetActive(!playerPosMsgCountText.gameObject.activeSelf);
            }


            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
            Vector2 mouseGamePosition = gameArea.transform.InverseTransformPoint(mouseWorldPosition);

            float yDiff = (Input.mousePosition.y - (Screen.height / 2)) * inputMultiplier;
            float xDiff = (Input.mousePosition.x - (Screen.width / 2)) * inputMultiplier;

            float spaceshipDirection = Mathf.Atan2(yDiff, xDiff);

            float targetX = mouseGamePosition.x, targetY = mouseGamePosition.y;

            spaceships[playerNumber].GetComponent<SpaceshipDrawer>().SetRotation(spaceshipDirection * 180.0f / Mathf.PI);

            bool sendSomething = false;

            if (Input.GetMouseButtonDown(0))
            {
                ShootNormalProjectileAction.StartShootNormalProjectileAction(networkHandler.builder);
                ShootNormalProjectileAction.AddFix(networkHandler.builder, ShootNormalProjectileActionFix.CreateShootNormalProjectileActionFix(networkHandler.builder, targetX, targetY));
                var action = ShootNormalProjectileAction.EndShootNormalProjectileAction(networkHandler.builder).Value;

                ClientActionMessage.StartClientActionMessage(networkHandler.builder);
                ClientActionMessage.AddContentType(networkHandler.builder, ClientAction.ShootNormalProjectileAction);
                ClientActionMessage.AddContent(networkHandler.builder, action);
                var message = ClientActionMessage.EndClientActionMessage(networkHandler.builder);

                networkHandler.Send(message);
                sendSomething = true;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                ShootBouncingProjectileAction.StartShootBouncingProjectileAction(networkHandler.builder);
                ShootBouncingProjectileAction.AddFix(networkHandler.builder, ShootBouncingProjectileActionFix.CreateShootBouncingProjectileActionFix(networkHandler.builder, targetX, targetY));
                var action = ShootBouncingProjectileAction.EndShootBouncingProjectileAction(networkHandler.builder).Value;

                ClientActionMessage.StartClientActionMessage(networkHandler.builder);
                ClientActionMessage.AddContentType(networkHandler.builder, ClientAction.ShootBouncingProjectileAction);
                ClientActionMessage.AddContent(networkHandler.builder, action);
                var message = ClientActionMessage.EndClientActionMessage(networkHandler.builder);

                networkHandler.Send(message);
                sendSomething = true;
            }

            timeToSendVelocity -= Time.deltaTime;

            if (timeToSendVelocity <= 0)
            {
                SendVelocity();

                do
                {
                    timeToSendVelocity += 0.05f;
                } while (timeToSendVelocity <= 0);

                sendSomething = true;
            }

            if (sendSomething)
            {
                networkHandler.Listen();
                networkHandler.DelayListen();
            }
        }

        if (clientState == ClientState.Running)
        {
            timeToShowPlayerPositionMessageCount -= Time.deltaTime;
            if (timeToShowPlayerPositionMessageCount <= 0)
            {
                playerPosMsgCountText.text = Convert.ToString(playerPositionMessageCount);
                playerPositionMessageCount = 0;
                do
                {
                    timeToShowPlayerPositionMessageCount += 1;
                } while (timeToShowPlayerPositionMessageCount <= 0);
            }

            CheckWarningState();
            UpdateTimeToDraw();
        }
    }

    private void SendVelocity()
    {
        float yDiff = (Input.mousePosition.y - (Screen.height / 2)) * inputMultiplier;
        float xDiff = (Input.mousePosition.x - (Screen.width / 2)) * inputMultiplier;

        float spaceshipDirection = Mathf.Atan2(yDiff, xDiff);

        ClientInputMessage.StartClientInputMessage(networkHandler.builder);

#if UNITY_EDITOR || UNITY_STANDALONE
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
#elif UNITY_ANDROID
        float inputX = Input.acceleration.x * 3;
        if (inputX > 1)
            {
            inputX = 1;
        }
        else if (inputX < -1)
        {
            inputX = -1;
        }

        float inputY = Input.acceleration.y * 3;
        if (inputY > 1)
        {
            inputY = 1;
        }
        else if (inputY < -1)
        {
            inputY = -1;
        }
#endif
        ClientInputMessage.AddFix(networkHandler.builder, ClientInputMessageFix.CreateClientInputMessageFix(
        networkHandler.builder, inputX, inputY, spaceshipDirection));
        var clientInputMessage = ClientInputMessage.EndClientInputMessage(networkHandler.builder);
        networkHandler.Send(clientInputMessage);
    }
}
