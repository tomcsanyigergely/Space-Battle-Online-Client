using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UdpMessages.ServerClientMessages;
using UdpMessages.ClientServerMessages;
using UnityEngine.SceneManagement;
using System;
using TMPro;

using UInt8 = System.Byte;

public partial class GameManager
{
    public void OnReceived(InitGameMessage initGameMessage)
    {
        gameSystem.InGame(initGameMessage);
    }

    private void InitializeGameState(InitGameMessage initGameMessage)
    {
        var fix = initGameMessage.Fix.Value;

        playerNumber = fix.PlayerNumber;
        teamSize = fix.TeamSize;

        spaceshipRespawnTime = fix.SpaceshipRespawnTime;
        spaceshipMaxLifePoints = fix.SpaceshipMaxLifePoints;
        spaceshipRadius = fix.SpaceshipRadius;

        normalProjectileRadius = fix.NormalProjectileRadius;
        normalProjectileSpeed = fix.NormalProjectileSpeed;
        normalProjectileClipSize = fix.NormalProjectileClipSize;
        normalProjectileReloadDelay = fix.NormalProjectileReloadDelay;
        normalProjectileReloadSpeed = fix.NormalProjectileReloadSpeed;

        bouncingProjectileRadius = fix.BouncingProjectileRadius;
        bouncingProjectileSpeed = fix.BouncingProjectileSpeed;
        bouncingProjectileClipSize = fix.BouncingProjectileClipSize;
        bouncingProjectileAutoReloadSpeed = fix.BouncingProjectileAutoReloadSpeed;

        controlPointRadius = fix.ControlPointRadius;
        controlPointTakingProgression = fix.ControlPointCapturingLimit;
        controlPointProgressionSpeed = fix.ControlPointCapturingSpeed;
        controlPointTimeBeforeCooldown = fix.ControlPointTimeBeforeCooldown;
        controlPointCooldownSpeed = fix.ControlPointCooldownSpeed;
        maxControlProgression = fix.ControlProgressionGoal;
        controlProgressionSpeed = fix.ControlProgressionSpeed;
        controlFinalizationDuration = fix.ReinstallDuration;
        timeToDraw = fix.TimeToDraw;

        if (playerNumber / teamSize == 0)
        {
            gameArea.transform.rotation = Quaternion.Euler(0, 0, 0);
            playerColors[0] = Config.PLAYER_COLOR;
            playerColors[1] = Config.ENEMY_COLOR;

            controlPointColors[0] = Config.PLAYER_CONTROL_COLOR;
            controlPointColors[1] = Config.ENEMY_CONTROL_COLOR;

            respawnPointColors[0] = Config.PLAYER_RESPAWN_POINT_COLOR;
            respawnPointColors[1] = Config.ENEMY_RESPAWN_POINT_COLOR;

            inputMultiplier = 1;

            controlledPointsTexts[0] = playerControlledPointsText;
            controlledPointsTexts[1] = enemyControlledPointsText;

            controlledPointsBackgrounds[0] = playerControlledPointsBackground;
            controlledPointsBackgrounds[1] = enemyControlledPointsBackground;

            controlProgressions[0] = playerControlProgression;
            controlProgressions[1] = enemyControlProgression;

            progressionDatas[0] = playerProgressionData;
            progressionDatas[1] = enemyProgressionData;
        }
        else {
            gameArea.transform.rotation = Quaternion.Euler(0, 0, 180);
            playerColors[0] = Config.ENEMY_COLOR;
            playerColors[1] = Config.PLAYER_COLOR;

            controlPointColors[0] = Config.ENEMY_CONTROL_COLOR;
            controlPointColors[1] = Config.PLAYER_CONTROL_COLOR;

            respawnPointColors[0] = Config.ENEMY_RESPAWN_POINT_COLOR;
            respawnPointColors[1] = Config.PLAYER_RESPAWN_POINT_COLOR;

            inputMultiplier = -1;

            controlledPointsTexts[0] = enemyControlledPointsText;
            controlledPointsTexts[1] = playerControlledPointsText;

            controlledPointsBackgrounds[0] = enemyControlledPointsBackground;
            controlledPointsBackgrounds[1] = playerControlledPointsBackground;

            controlProgressions[0] = enemyControlProgression;
            controlProgressions[1] = playerControlProgression;

            progressionDatas[0] = enemyProgressionData;
            progressionDatas[1] = playerProgressionData;
        }

        playerUsernameTexts = new TextMeshProUGUI[2 * teamSize];
        playerDisconnectedIcons = new Image[2 * teamSize]; // playerDisconnectedIcons[playerNumber] will be null
        playerConnectingIcons = new Image[2 * teamSize]; // playerConnectingIcons[playerNumber] will be null

        playerUsernameTexts[playerNumber] = allUsernameTexts[0];

        UInt8 nextTeammateUsernameTextIndex = 1;
        UInt8 nextEnemyUsernameTextIndex = 3;

        for (UInt8 playerIndex = 0; playerIndex < 2 * teamSize; playerIndex++)
        {
            if (playerIndex != playerNumber)
            {
                if (playerIndex / teamSize == playerNumber / teamSize) // if is it a teammate
                {
                    playerUsernameTexts[playerIndex] = allUsernameTexts[nextTeammateUsernameTextIndex];
                    playerDisconnectedIcons[playerIndex] = allDisconnectedIcons[nextTeammateUsernameTextIndex - 1]; // subtracting 1, because there is no icon for our player
                    playerConnectingIcons[playerIndex] = allConnectingIcons[nextTeammateUsernameTextIndex - 1];
                    nextTeammateUsernameTextIndex++;
                }
                else
                {
                    playerUsernameTexts[playerIndex] = allUsernameTexts[nextEnemyUsernameTextIndex];
                    playerDisconnectedIcons[playerIndex] = allDisconnectedIcons[nextEnemyUsernameTextIndex - 1]; // subtracting 1, because there is no icon for our player
                    playerConnectingIcons[playerIndex] = allConnectingIcons[nextEnemyUsernameTextIndex - 1];
                    nextEnemyUsernameTextIndex++;
                }
            }
        }

        normalProjectileAmmoText.GetComponent<Ammo>().Init(normalProjectileClipSize, normalProjectileReloadSpeed);
        bouncingProjectileAmmoText.GetComponent<Ammo>().Init(bouncingProjectileClipSize, bouncingProjectileAutoReloadSpeed);

        normalProjectileAmmoText.GetComponent<Ammo>().SetAmmo(fix.NormalProjectileAmmo);
        normalProjectileAmmoText.GetComponent<Ammo>().SetReloadTimeRemaining(fix.NormalProjectileReloadTimeRemaining);
        bouncingProjectileAmmoText.GetComponent<Ammo>().SetAmmo(fix.BouncingProjectileAmmo);
        bouncingProjectileAmmoText.GetComponent<Ammo>().SetReloadTimeRemaining(fix.BouncingProjectileReloadTimeRemaining);

        controlProgressions[0].GetComponent<ControlProgression>().Init(maxControlProgression, controlProgressionSpeed);
        controlProgressions[1].GetComponent<ControlProgression>().Init(maxControlProgression, controlProgressionSpeed);
        controlProgressions[0].GetComponent<ControlProgression>().SetControlProgression(fix.TeamOneControlProgression);
        controlProgressions[1].GetComponent<ControlProgression>().SetControlProgression(fix.TeamTwoControlProgression);
        progressionDatas[0].GetComponent<ReinstallTime>().SetRemainingTime(fix.TeamOneReinstallRemaining > 0 ? fix.TeamOneReinstallRemaining : 0);
        progressionDatas[1].GetComponent<ReinstallTime>().SetRemainingTime(fix.TeamTwoReinstallRemaining > 0 ? fix.TeamTwoReinstallRemaining : 0);

        respawnPoints.Add(Instantiate(respawnPointPrefab));
        respawnPoints[0].transform.parent = gameArea.transform;
        respawnPoints[0].transform.localPosition = new Vector3(fix.TeamOneRespawnPoint.Position.X, fix.TeamOneRespawnPoint.Position.Y, Config.RESPAWN_POINT_Z_DEPTH);
        respawnPoints[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
        respawnPoints[0].transform.localScale = new Vector3(fix.TeamOneRespawnPoint.Width, fix.TeamOneRespawnPoint.Height, 1);
        respawnPoints[0].GetComponent<Renderer>().material.color = respawnPointColors[0];

        respawnPoints.Add(Instantiate(respawnPointPrefab));
        respawnPoints[1].transform.parent = gameArea.transform;
        respawnPoints[1].transform.localPosition = new Vector3(fix.TeamTwoRespawnPoint.Position.X, fix.TeamTwoRespawnPoint.Position.Y, Config.RESPAWN_POINT_Z_DEPTH);
        respawnPoints[1].transform.localRotation = Quaternion.Euler(0, 0, 0);
        respawnPoints[1].transform.localScale = new Vector3(fix.TeamTwoRespawnPoint.Width, fix.TeamTwoRespawnPoint.Height, 1);
        respawnPoints[1].GetComponent<Renderer>().material.color = respawnPointColors[1];

        ///////////////////////////////////////
        ///

        for (int i = 0; i < initGameMessage.SpaceshipsLength; i++)
        {
            playerUsernameTexts[i].text = initGameMessage.Spaceships(i).Value.Username;
            floatingUsernameTexts[i].text = initGameMessage.Spaceships(i).Value.Username;
            if (i / teamSize == playerNumber / teamSize)
            {
                floatingUsernameTexts[i].color = teammateAliveUsernameColor;
            }
            else
            {
                floatingUsernameTexts[i].color = opponentAliveUsernameColor;
            }

            if (i != playerNumber)
            {
                float offsetX = playerUsernameTexts[i].rectTransform.sizeDelta.x / 2.0f - playerUsernameTexts[i].preferredWidth;
                if (i / teamSize == playerNumber / teamSize)
                {
                    offsetX = offsetX - 10;
                }
                else
                {
                    offsetX = -offsetX + 20;
                }
                playerDisconnectedIcons[i].transform.localPosition = new Vector3(offsetX, 0, 0);
                playerConnectingIcons[i].transform.localPosition = new Vector3(offsetX, 0, 0);
                playerConnectingIcons[i].gameObject.SetActive(!initGameMessage.Spaceships(i).Value.IsConnected);
            }

            GameObject spaceshipShadow = new GameObject();
            spaceshipShadows.Add(spaceshipShadow);
            spaceshipShadow.transform.parent = gameArea.transform;

            GameObject spaceship = Instantiate(spaceshipPrefab);
            spaceships.Add(spaceship);
            spaceship.transform.parent = gameArea.transform;
            spaceship.transform.localRotation = Quaternion.Euler(0, 0, 0);
            spaceship.transform.localScale = new Vector3(1, 1, 1);
            spaceship.GetComponent<SpaceshipDrawer>().Init(playerColors, spaceshipRadius);
            spaceship.GetComponent<HealthBar>().Init(new Vector3(0, (spaceshipRadius + Config.HEALTH_BAR_Y_OFFSET) * inputMultiplier, Config.HEALTH_BAR_Z_DEPTH), Vector3.Scale(Config.HEALTH_BAR_SCALE, new Vector3(inputMultiplier, inputMultiplier, 1)));
            spaceship.GetComponent<Health>().Init(spaceshipMaxLifePoints);
            spaceship.GetComponent<Ownable>().SetOwner((byte)(i / teamSize));
            spaceship.GetComponent<Interpolate>().SetInterpolateRotation(i != playerNumber);

            if (i == playerNumber)
            {
                spaceships[playerNumber].GetComponent<Health>().onHealthChanged += playerHealthBar.GetComponent<HealthBar>().OnHealthChanged;
                spaceships[playerNumber].GetComponent<Health>().onHealthChanged += playerHealthBar.GetComponent<HealthDrawer>().OnHealthChanged;
            }

            var spaceshipState = initGameMessage.Spaceships(i).Value.State;

            if (spaceshipState.HasValue)
            {
                floatingUsernameTexts[i].text = initGameMessage.Spaceships(i).Value.Username;

                spaceship.transform.localPosition = new Vector3(spaceshipState.Value.Position.X, spaceshipState.Value.Position.Y, Config.SPACESHIP_Z_DEPTH);
                spaceship.GetComponent<SpaceshipDrawer>().SetRotation(spaceshipState.Value.Direction * 180f / Mathf.PI);
                spaceship.GetComponent<Health>().SetHealthPoints(spaceshipState.Value.LifePoints);
                spaceship.GetComponent<Health>().SetHealingSpeed(spaceshipState.Value.HealingSpeed);

                if (i / teamSize == playerNumber / teamSize)
                {
                    playerUsernameTexts[i].color = teammateAliveUsernameColor;
                }
                else
                {
                    playerUsernameTexts[i].color = opponentAliveUsernameColor;
                }
            }
            else
            {
                floatingUsernameTexts[i].text = "";

                spaceship.GetComponent<Health>().SetHealthPoints(0);
                spaceship.transform.Find("Empty").Find("Empty").Find("Model").gameObject.SetActive(false);
                spaceship.transform.Find("Empty").Find("Health Bar").gameObject.SetActive(false);

                if (i / teamSize == playerNumber / teamSize)
                {
                    playerUsernameTexts[i].color = teammateDeadUsernameColor;
                }
                else
                {
                    playerUsernameTexts[i].color = opponentDeadUsernameColor;
                }
            }
        }

        ///////////////////////////////

        for (int i = 0; i < initGameMessage.ObstaclesLength; ++i)
        {
            var obstacleData = initGameMessage.Obstacles(i).Value;

            GameObject obstacle = new GameObject();
            obstacles.Add(obstacle);
            obstacle.transform.parent = gameArea.transform;
            obstacle.transform.localPosition = new Vector3(obstacleData.Position.Value.X, obstacleData.Position.Value.Y, Config.OBSTACLE_Z_DEPTH);
            obstacle.transform.localRotation = Quaternion.Euler(0, 0, 0);
            obstacle.transform.localScale = new Vector3(1, 1, 1);

            MeshRenderer meshRenderer = obstacle.AddComponent<MeshRenderer>();
            meshRenderer.material = asteroidMaterial;

            MeshFilter meshFilter = obstacle.AddComponent<MeshFilter>();

            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[1 + obstacleData.ColliderLength];
            vertices[0] = new Vector3(0, 0, 0);
            for (UInt8 pointIndex = 0; pointIndex < obstacleData.ColliderLength; pointIndex++) {
                Position pointPosition = obstacleData.Collider(pointIndex).Value;
                vertices[1 + pointIndex] = new Vector3(pointPosition.X, pointPosition.Y, 0);
            }
            mesh.vertices = vertices;

            int[] triangles = new int[3 * obstacleData.ColliderLength];
            for (UInt8 pointIndex = 0; pointIndex < obstacleData.ColliderLength; pointIndex++) {
                triangles[3 * pointIndex + 0] = 0;
                triangles[3 * pointIndex + 1] = 1 + pointIndex;
                triangles[3 * pointIndex + 2] = 1 + ((1 + pointIndex) % obstacleData.ColliderLength);
            }
            mesh.triangles = triangles;

            Vector2[] uv = new Vector2[vertices.Length];
            for (int j = 0; j < uv.Length; j++)
            {
                uv[j] = new Vector2(obstacleData.Position.Value.X + vertices[j].x, obstacleData.Position.Value.Y + vertices[j].y);
            }
            mesh.uv = uv;

            meshFilter.mesh = mesh;
        }

        for (int i = 0; i < initGameMessage.ProjectilesLength; ++i)
        {
            var projectileState = initGameMessage.Projectiles(i).Value.State;

            GameObject projectile = Instantiate(projectilePrefab);
            projectiles.Add(projectile);
            projectile.transform.parent = gameArea.transform;
            projectile.transform.localRotation = Quaternion.Euler(0, 0, 0);
            projectile.transform.localScale = new Vector3(1, 1, 1);

            if (projectileState.HasValue)
            {
                switch (projectileState.Value.Type)
                {
                    case ProjectileType.Normal:
                        projectile.transform.Find("Normal").gameObject.SetActive(true);
                        projectile.transform.Find("Bouncing").gameObject.SetActive(false);
                        break;
                    case ProjectileType.Bouncing:
                        projectile.transform.Find("Normal").gameObject.SetActive(false);
                        projectile.transform.Find("Bouncing").gameObject.SetActive(true);
                        break;
                }
                projectile.GetComponent<ProjectileDrawer>().Init(playerColors, normalProjectileRadius, bouncingProjectileRadius);
                projectile.GetComponent<Ownable>().SetOwner(projectileState.Value.Owner);
                projectile.GetComponent<Velocity>().SetDirection(projectileState.Value.Direction);
                projectile.GetComponent<Velocity>().Speed = projectileState.Value.Type == ProjectileType.Normal ? normalProjectileSpeed : bouncingProjectileSpeed;
                projectile.transform.localPosition = new Vector3(projectileState.Value.Position.X, projectileState.Value.Position.Y, Config.BALL_Z_DEPTH);
            }
            else
            {
                projectile.GetComponent<ProjectileDrawer>().Init(playerColors, normalProjectileRadius, bouncingProjectileRadius);
                projectile.SetActive(false);
            }
        }

        int[] numberOfControlPoints = { 0, 0 };

        for (int i = 0; i < initGameMessage.ControlPointStatesLength; ++i)
        {
            var controlPointState = initGameMessage.ControlPointStates(i).Value;
            var controlPointStateFix = controlPointState.Fix.Value;

            GameObject controlPoint = Instantiate(controlPointPrefab);
            controlPoints.Add(controlPoint);
            controlPoint.transform.parent = gameArea.transform;
            controlPoint.transform.localPosition = new Vector3(controlPointStateFix.Position.X, controlPointStateFix.Position.Y, Config.CONTROL_POINT_Z_DEPTH);
            controlPoint.transform.localRotation = Quaternion.Euler(0, 0, 0);
            controlPoint.transform.localScale = new Vector3(1, 1, 1);
            controlPoint.GetComponent<ControlPointDrawer>().Init(controlPointColors, controlPointRadius);
            controlPoint.GetComponent<CaptureBar>().Init(new Vector3(0, 0, Config.CAPTURE_BAR_Z_DEPTH), Vector3.Scale(Config.CAPTURE_BAR_SCALE, new Vector3(inputMultiplier, inputMultiplier, 1)), playerColors);
            controlPoint.GetComponent<ControlPoint>().Init(controlPointTakingProgression, controlPointProgressionSpeed, controlPointTimeBeforeCooldown, controlPointCooldownSpeed);
            controlPoint.GetComponent<ControlPoint>().SetState(
                new bool[] { controlPointStateFix.ContestedByTeamOne, controlPointStateFix.ContestedByTeamTwo },
                controlPointStateFix.ProgressedTeam,
                controlPointStateFix.Progression,
                controlPointStateFix.TimeBeforeCooldown > 0 ? controlPointStateFix.TimeBeforeCooldown : 0
            );

            if (controlPointState.Owner.HasValue)
            {
                controlPoint.GetComponent<Ownable>().SetOwner(controlPointState.Owner.Value.Value);
                numberOfControlPoints[controlPointState.Owner.Value.Value]++;
            }
        }

        if (numberOfControlPoints[0] > numberOfControlPoints[1])
        {
            controlProgressions[0].GetComponent<ControlProgression>().EnableProgress(true);
            controlProgressions[1].GetComponent<ControlProgression>().EnableProgress(false);
        }
        else if (numberOfControlPoints[1] > numberOfControlPoints[0])
        {
            controlProgressions[0].GetComponent<ControlProgression>().EnableProgress(false);
            controlProgressions[1].GetComponent<ControlProgression>().EnableProgress(true);
        }
        else
        {
            controlProgressions[0].GetComponent<ControlProgression>().EnableProgress(false);
            controlProgressions[1].GetComponent<ControlProgression>().EnableProgress(false);
        }

        ClientReadyMessage.StartClientReadyMessage(networkHandler.builder);
        var clientReadyMessage = ClientReadyMessage.EndClientReadyMessage(networkHandler.builder);
        networkHandler.Send(clientReadyMessage);

        messageText.text = "Waiting for players...";

        clientState = ClientState.WaitingForOpponent;

        UpdateAttackedIcons();
        UpdateControlledPoints();
        UpdatePlayerAttackingAudio();

        timeToDrawText.text = TimeSpan.FromSeconds(Mathf.CeilToInt(timeToDraw)).ToString(@"mm\:ss");

        mainCamera.transform.position = new Vector3(spaceships[playerNumber].transform.position.x, spaceships[playerNumber].transform.position.y, -25);
        backgroundCamera.GetComponent<Spin>().enabled = false;
        backgroundCamera.transform.localEulerAngles = new Vector3(-5 * spaceships[playerNumber].transform.localPosition.y, 5 * spaceships[playerNumber].transform.localPosition.x, 0) * inputMultiplier;

        UIUtilities.Show(inGameHUD);
    }

    public void OnReceived(MatchStartMessage matchStartMessage) {
        var fix = matchStartMessage.Fix.Value;

        if (clientState == ClientState.WaitingForOpponent)
        {
            for (UInt8 playerIndex = 0; playerIndex < 2 * teamSize; playerIndex++)
            {
                if (playerIndex != playerNumber)
                {
                    if (playerConnectingIcons[playerIndex].gameObject.activeSelf)
                    {
                        playerConnectingIcons[playerIndex].gameObject.SetActive(false);
                        playerDisconnectedIcons[playerIndex].gameObject.SetActive(true);
                    }
                }
            }
        }

        if (fix.SecondsToWait == 0)
        {
            ingameMusic.Play();
            clientState = ClientState.Running;
            messageText.text = "";
            controlProgressions[0].GetComponent<ControlProgression>().enabled = true;
            controlProgressions[1].GetComponent<ControlProgression>().enabled = true;
            normalProjectileAmmoText.GetComponent<Ammo>().enabled = true;
            bouncingProjectileAmmoText.GetComponent<Ammo>().enabled = true;
        }
        else {
            clientState = ClientState.Countdown;
            StartCoroutine(Countdown(Convert.ToInt16(fix.SecondsToWait)));
        }
    }

    public void OnReceived(MatchFinishedMessage matchFinishedMessage)
    {
        clientState = ClientState.Finished;

        continueButton.gameObject.SetActive(true);
        interpolateButton.gameObject.SetActive(false);
        if (matchFinishedMessage.Winner.HasValue) {
            UInt8 winner = matchFinishedMessage.Winner.Value.Value;

            if (winner == playerNumber / teamSize)
            {
                messageText.text = "YOU WIN!";
            }
            else
            {
                messageText.text = "YOU LOSE!";
            }
        }
        else
        {
            messageText.text = "DRAW!";
        }

        StopGame();
    }

    public void OnReceived(MatchCancelledMessage matchCancelledMessage)
    {
        clientState = ClientState.Finished;

        continueButton.gameObject.SetActive(true);
        interpolateButton.gameObject.SetActive(false);

        messageText.text = "Match has been cancelled!";

        StopGame();
    }

    private void StopGame()
    {
        foreach (GameObject spaceship in spaceships)
        {
            spaceship.GetComponent<Health>().SetHealingSpeed(0);
        }

        foreach (GameObject projectile in projectiles)
        {
            projectile.GetComponent<Velocity>().Speed = 0;
        }

        foreach (GameObject controlPoint in controlPoints)
        {
            controlPoint.GetComponent<ControlPoint>().Stop();
        }

        UIUtilities.Hide(inGameHUD);
    }

    public void OnReceived(PlayerConnectionChangedMessage playerConnectionChangedMessage)
    {
        var fix = playerConnectionChangedMessage.Fix.Value;

        if (fix.PlayerNumber != playerNumber) // defensive check
        {
            if (clientState != ClientState.WaitingForOpponent)
            {
                playerDisconnectedIcons[fix.PlayerNumber].gameObject.SetActive(!fix.IsConnected);
            }
            else
            {
                playerConnectingIcons[fix.PlayerNumber].gameObject.SetActive(!fix.IsConnected);
            }
        }
    }

    public void OnReceived(PlayerPositionMessage playerPositionMessage)
    {
        playerPositionMessageCount++;

        for (int i = 0; i < playerPositionMessage.DataLength; i++)
        {
            var playerPositionData = playerPositionMessage.Data(i).Value;

            spaceshipShadows[i].transform.localPosition = new Vector3(playerPositionData.Position.X, playerPositionData.Position.Y, Config.SPACESHIP_Z_DEPTH);
            spaceshipShadows[i].transform.localEulerAngles = new Vector3(0, 0, playerPositionData.Direction * 180.0f / Mathf.PI);

            if (spaceships[i].transform.Find("Empty").Find("Empty").Find("Model").gameObject.activeSelf)
            {
                spaceships[i].GetComponent<Interpolate>().AddPoint(spaceshipShadows[i].transform.localPosition, spaceshipShadows[i].transform.localEulerAngles.z, playerPositionMessage.TickCounter);
            }
        }
    }

    private void ProcessAction(Action action)
    {
        if (Interpolate.interpolate)
        {
            actions.AddLast(Tuple.Create(Time.time + Interpolate.delay, action));
        }
        else
        {
            action();
        }
    }

    public void OnReceived(SpaceshipRespawnedMessage spaceshipRespawnedMessage)
    {
        var fix = spaceshipRespawnedMessage.Fix.Value;

        Action action = () => { ProcessSpaceshipRespawnedMessage(fix.Owner); };
        ProcessAction(action);
    }

    private void ProcessSpaceshipRespawnedMessage(UInt8 owner)
    {
        spaceships[owner].GetComponent<Interpolate>().Clear();
        spaceships[owner].transform.localPosition = spaceshipShadows[owner].transform.localPosition;
        spaceships[owner].GetComponent<SpaceshipDrawer>().SetRotation(spaceshipShadows[owner].transform.localEulerAngles.z);
        spaceships[owner].GetComponent<Health>().SetHealthPoints(spaceshipMaxLifePoints);
        spaceships[owner].transform.Find("Empty").Find("Empty").Find("Model").gameObject.SetActive(true);
        spaceships[owner].transform.Find("Empty").Find("Health Bar").gameObject.SetActive(true);

        if (owner / teamSize == playerNumber / teamSize)
        {
            playerUsernameTexts[owner].color = teammateAliveUsernameColor;
        }
        else
        {
            playerUsernameTexts[owner].color = opponentAliveUsernameColor;
        }

        floatingUsernameTexts[owner].text = playerUsernameTexts[owner].text;
    }

    public void OnReceived(SpaceshipLifePointsChangedMessage spaceshipLifePointsChangedMessage)
    {
        var fix = spaceshipLifePointsChangedMessage.Fix.Value;

        Action action = () => { ProcessSpaceshipLifePointsChangedMessage(fix.Owner, fix.LifePoints); };
        ProcessAction(action);
    }

    private void ProcessSpaceshipLifePointsChangedMessage(UInt8 owner, float lifePoints)
    {
        if (owner == playerNumber && lifePoints > 0)
        {
            hurtAudio.Play();
        }

        spaceships[owner].GetComponent<Health>().SetHealthPoints(lifePoints);
        spaceships[owner].GetComponent<Damageable>().Damage();
    }

    public void OnReceived(SpaceshipDestroyedMessage spaceshipDestroyedMessage)
    {
        var fix = spaceshipDestroyedMessage.Fix.Value;

        Action action = () => { ProcessSpaceshipDestroyedMessage(fix.Owner); };
        ProcessAction(action);
    }

    private void ProcessSpaceshipDestroyedMessage(UInt8 owner)
    {
        explosionAudio.Play();

        spaceships[owner].GetComponent<Health>().SetHealingSpeed(0);
        spaceships[owner].GetComponent<Health>().SetHealthPoints(0);
        spaceships[owner].GetComponent<SpaceshipDrawer>().Explode();
        spaceships[owner].transform.Find("Empty").Find("Empty").Find("Model").gameObject.SetActive(false);
        spaceships[owner].transform.Find("Empty").Find("Health Bar").gameObject.SetActive(false);

        if (owner / teamSize == playerNumber / teamSize)
        {
            playerUsernameTexts[owner].color = teammateDeadUsernameColor;
        }
        else
        {
            playerUsernameTexts[owner].color = opponentDeadUsernameColor;
        }

        if (playerNumber == owner)
        {
            StartCoroutine(RespawnCountdown(spaceshipRespawnTime));
        }

        floatingUsernameTexts[owner].text = "";
    }

    public void OnReceived(SpaceshipHealingChangedMessage spaceshipHealingChangedMessage)
    {
        var fix = spaceshipHealingChangedMessage.Fix.Value;

        Action action = () => { ProcessSpaceshipHealingChangedMessage(fix.Owner, fix.HealingSpeed, fix.LifePoints); };
        ProcessAction(action);
    }

    private void ProcessSpaceshipHealingChangedMessage(UInt8 owner, float healingSpeed, float lifePoints)
    {
        spaceships[owner].GetComponent<Health>().SetHealingSpeed(healingSpeed);
        spaceships[owner].GetComponent<Health>().SetHealthPoints(lifePoints);
    }

    public void OnReceived(SpaceshipHitMessage spaceshipHitMessage)
    {
        Action action = () => { ProcessSpaceshipHitMessage(); };
        ProcessAction(action);
    }

    private void ProcessSpaceshipHitMessage()
    {
        enemyHitAudio.Play();
    }

    public void OnReceived(ProjectileShotMessage projectileShotMessage)
    {
        var fix = projectileShotMessage.Fix.Value;

        switch (fix.Type)
        {
            case ProjectileType.Normal:
                if (projectileShotMessage.Ammo.HasValue)
                {
                    normalProjectileAmmoText.GetComponent<Ammo>().SetAmmo(projectileShotMessage.Ammo.Value.Value);
                }
                if (projectileShotMessage.ReloadTimeRemaining.HasValue)
                {
                    normalProjectileAmmoText.GetComponent<Ammo>().SetReloadTimeRemaining(projectileShotMessage.ReloadTimeRemaining.Value.Value);
                }
                break;
            case ProjectileType.Bouncing:
                if (projectileShotMessage.Ammo.HasValue)
                {
                    bouncingProjectileAmmoText.GetComponent<Ammo>().SetAmmo(projectileShotMessage.Ammo.Value.Value);
                }
                if (projectileShotMessage.ReloadTimeRemaining.HasValue)
                {
                    bouncingProjectileAmmoText.GetComponent<Ammo>().SetReloadTimeRemaining(projectileShotMessage.ReloadTimeRemaining.Value.Value);
                }
                break;
        }

        Action action = () => { ProcessProjectileShotMessage(fix.ProjectileId, fix.Owner, fix.Position.X, fix.Position.Y, fix.Direction, fix.Type); };
        ProcessAction(action);
    }

    private void ProcessProjectileShotMessage(UInt8 projectileId, UInt8 owner, float posX, float posY, float direction, ProjectileType type)
    {
        GameObject projectile = projectiles[projectileId];

        switch (type)
        {
            case ProjectileType.Normal:
                if (owner == playerNumber)
                {
                    normalProjectileAudio.Play();
                }
                projectile.transform.Find("Normal").gameObject.SetActive(true);
                projectile.transform.Find("Bouncing").gameObject.SetActive(false);
                break;
            case ProjectileType.Bouncing:
                projectile.transform.Find("Normal").gameObject.SetActive(false);
                projectile.transform.Find("Bouncing").gameObject.SetActive(true);
                break;
        }

        projectile.transform.localPosition = new Vector3(posX, posY, Config.BALL_Z_DEPTH);
        projectile.GetComponent<Ownable>().SetOwner(owner);
        projectile.GetComponent<Velocity>().SetDirection(direction);
        projectile.GetComponent<Velocity>().Speed = type == ProjectileType.Normal ? normalProjectileSpeed : bouncingProjectileSpeed;
        projectile.SetActive(true);
    }

    public void OnReceived(ProjectileDirectionChangedMessage projectileDirectionChangedMessage)
    {
        var fix = projectileDirectionChangedMessage.Fix.Value;

        Action action = () => { ProcessProjectileDirectionChangedMessage(fix.ProjectileId, fix.Position.X, fix.Position.Y, fix.Direction); };
        ProcessAction(action);
    }

    private void ProcessProjectileDirectionChangedMessage(UInt8 projectileId, float posX, float posY, float direction)
    {
        GameObject projectile = projectiles[projectileId];

        projectile.transform.localPosition = new Vector3(posX, posY, Config.BALL_Z_DEPTH);
        projectile.GetComponent<Velocity>().SetDirection(direction);
    }

    public void OnReceived(ProjectileDisappearedMessage projectileDisappearedMessage)
    {
        var fix = projectileDisappearedMessage.Fix.Value;

        if (projectileDisappearedMessage.Collision.HasValue) {
            Action action = () => { ProcessProjectileDisappearedMessage(fix.ProjectileId, projectileDisappearedMessage.Collision.Value.Fix.Value); };
            ProcessAction(action);
        }
        else
        {
            Action action = () => { ProcessProjectileDisappearedMessage(fix.ProjectileId); };
            ProcessAction(action);
        }
    }

    private void ProcessProjectileDisappearedMessage(UInt8 projectileId, UdpMessages.ServerClientMessages.CollisionFix collision)
    {
        projectiles[projectileId].SetActive(false);

        ParticleSystem flashParticle = flashParticles[nextFlashParticleIndex];
        flashParticle.transform.localPosition = new Vector3(collision.CollisionPoint.X, collision.CollisionPoint.Y, Config.BALL_Z_DEPTH);
        flashParticle.transform.localRotation = Quaternion.Euler(0, 0, collision.SurfaceDirection * 180.0f / Mathf.PI - 60);
        flashParticle.Play();

        nextFlashParticleIndex = (nextFlashParticleIndex + 1) % flashParticles.Length;
    }

    private void ProcessProjectileDisappearedMessage(UInt8 projectileId)
    {
        projectiles[projectileId].SetActive(false);
    }

    public void OnReceived(ControlPointTakenMessage controlPointTakenMessage)
    {
        var fix = controlPointTakenMessage.Fix.Value;

        timeToDraw = fix.TimeToDraw;
        timeToDrawText.text = TimeSpan.FromSeconds(Mathf.CeilToInt(timeToDraw)).ToString(@"mm\:ss");

        if (fix.Team == playerNumber / teamSize)
        {
            playerTakenCPAudio.Play();
        }
        else
        {
            if (controlPoints[fix.ControlPointId].GetComponent<ControlPoint>().Owner.HasValue &&
                controlPoints[fix.ControlPointId].GetComponent<ControlPoint>().Owner.Value == playerNumber)
            {
                playerLostCPAudio.Play();
            }
            else
            {
                enemyTakenNeutralCPAudio.Play();
            }
        }

        if (controlPoints[fix.ControlPointId].GetComponent<ControlPoint>().Owner.HasValue)
        {
            progressionDatas[controlPoints[fix.ControlPointId].GetComponent<ControlPoint>().Owner.Value].GetComponent<ReinstallTime>().SetRemainingTime(controlFinalizationDuration);
        }
        controlPoints[fix.ControlPointId].GetComponent<Ownable>().SetOwner(fix.Team);

        controlProgressions[0].GetComponent<ControlProgression>().SetControlProgression(fix.TeamOneControlProgression);
        controlProgressions[1].GetComponent<ControlProgression>().SetControlProgression(fix.TeamTwoControlProgression);

        int[] numberOfControlPoints = { 0, 0 };

        for (int i = 0; i < controlPoints.Count; ++i)
        {
            if (controlPoints[i].GetComponent<ControlPoint>().Owner.HasValue)
            {
                numberOfControlPoints[controlPoints[i].GetComponent<ControlPoint>().Owner.Value]++;
            }
        }

        if (numberOfControlPoints[0] > numberOfControlPoints[1])
        {
            controlProgressions[0].GetComponent<ControlProgression>().EnableProgress(true);
            controlProgressions[1].GetComponent<ControlProgression>().EnableProgress(false);
        }
        else if (numberOfControlPoints[1] > numberOfControlPoints[0])
        {
            controlProgressions[0].GetComponent<ControlProgression>().EnableProgress(false);
            controlProgressions[1].GetComponent<ControlProgression>().EnableProgress(true);
        }
        else
        {
            controlProgressions[0].GetComponent<ControlProgression>().EnableProgress(false);
            controlProgressions[1].GetComponent<ControlProgression>().EnableProgress(false);
        }

        UpdateAttackedIcons();
        UpdateControlledPoints();
        UpdatePlayerAttackingAudio();
    }

    public void OnReceived(ControlPointContestingChangedMessage controlPointContestingChangedMessage)
    {
        var fix = controlPointContestingChangedMessage.Fix.Value;

        timeToDraw = fix.TimeToDraw;
        timeToDrawText.text = TimeSpan.FromSeconds(Mathf.CeilToInt(timeToDraw)).ToString(@"mm\:ss");

        controlPoints[fix.ControlPointId].GetComponent<ControlPoint>().ChangeContesting(fix.Team, fix.IsContesting, fix.ProgressedTeam, fix.Progression);

        UpdateAttackedIcons();
        UpdatePlayerAttackingAudio();
    }
}
