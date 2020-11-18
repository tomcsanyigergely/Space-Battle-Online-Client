using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UInt8 = System.Byte;

public class ControlPoint : MonoBehaviour
{
    public UnityAction<float, float> onProgressionChanged;
    public UnityAction<bool[]> onContestingChanged;
    public UnityAction<UInt8> onProgressedPlayerChanged;

    private float takingProgression;
    private float progressionSpeed;
    private float preCooldownDuration;
    private float cooldownSpeed;

    public UInt8? Owner { get; private set; }
    public bool[] IsContesting { get; private set; } = new bool[2];
    public UInt8 ProgressedPlayer { get; private set; }
    public float Progression { get; private set; }
    private float preCooldownRemainingTime;

    private bool stopped = false;

    private void Awake()
    {
        GetComponent<Ownable>().onOwnerChanged += OnOwnerChanged;
    }

    public void Init(float takingProgression, float progressionSpeed, float preCooldownDuration, float cooldownSpeed)
    {
        this.takingProgression = takingProgression;
        this.progressionSpeed = progressionSpeed;
        this.preCooldownDuration = preCooldownDuration;
        this.cooldownSpeed = cooldownSpeed;
    }

    public void SetState(bool[] isContesting, UInt8 progressedPlayer, float progression, float preCooldownRemainingTime)
    {
        IsContesting = isContesting;
        this.ProgressedPlayer = progressedPlayer;
        this.Progression = progression;
        this.preCooldownRemainingTime = preCooldownRemainingTime;

        onProgressionChanged(progression, takingProgression);
        onContestingChanged(isContesting);
        onProgressedPlayerChanged(progressedPlayer);
    }

    private void Update()
    {
        if (!stopped)
        {
            if (preCooldownRemainingTime > 0)
            {
                preCooldownRemainingTime -= Time.deltaTime;
                if (preCooldownRemainingTime < 0)
                {
                    preCooldownRemainingTime = 0;
                }
            }

            UInt8? capturingPlayer = null;

            if (IsContesting[0] && !IsContesting[1])
            {
                capturingPlayer = 0;
            }
            else if (IsContesting[1] && !IsContesting[0])
            {
                capturingPlayer = 1;
            }

            if (capturingPlayer.HasValue)
            {
                float deltaProgression = progressionSpeed * Time.deltaTime;

                if (Owner.HasValue)
                {
                    if (Owner.Value == capturingPlayer.Value)
                    {
                        Progression -= deltaProgression;
                        if (Progression < 0)
                        {
                            Progression = 0;
                        }
                    }
                    else
                    {
                        Progression += deltaProgression;
                        if (Progression > takingProgression)
                        {
                            Progression = takingProgression;
                        }
                    }
                }
                else
                {
                    if (ProgressedPlayer == capturingPlayer.Value)
                    {
                        Progression += deltaProgression;
                        if (Progression > takingProgression)
                        {
                            Progression = takingProgression;
                        }
                    }
                    else
                    {
                        Progression -= deltaProgression;
                        if (Progression < 0)
                        {
                            Progression *= -1;
                            ProgressedPlayer = capturingPlayer.Value;
                            onProgressedPlayerChanged(ProgressedPlayer);
                        }
                    }
                }

                onProgressionChanged(Progression, takingProgression);
            }
            else if (!IsContesting[0] && !IsContesting[1])
            {
                if (Progression > 0 && preCooldownRemainingTime == 0)
                {
                    Progression -= Time.deltaTime * cooldownSpeed;
                    if (Progression < 0)
                    {
                        Progression = 0;
                    }
                    onProgressionChanged(Progression, takingProgression);
                }
            }
        }
    }

    public void ChangeContesting(UInt8 playerNumber, bool isContesting, UInt8 progressedPlayer, float progression)
    {
        UInt8 previousProgressedPlayer = this.ProgressedPlayer;

        IsContesting[playerNumber] = isContesting;
        this.ProgressedPlayer = progressedPlayer;
        this.Progression = progression;

        if (progressedPlayer == playerNumber && !isContesting)
        {
            preCooldownRemainingTime = preCooldownDuration;
        }

        onProgressionChanged(progression, takingProgression);
        onContestingChanged(IsContesting);

        if (progressedPlayer != previousProgressedPlayer)
        {
            onProgressedPlayerChanged(progressedPlayer);
        }
    }

    private void OnOwnerChanged(UInt8 owner)
    {
        Owner = owner;
        Progression = 0;
    }

    public void Stop()
    {
        stopped = true;
    }
}
