using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UInt8 = System.Byte;

public class Ammo : MonoBehaviour
{
    public UnityAction<UInt8, UInt8> onAmmoChanged;

    private UInt8 clipSize;
    private float reloadSpeed;

    private UInt8 ammo = 0;
    private float reloadTimeRemaining = 0;

    public void Init(UInt8 clipSize, float reloadSpeed)
    {
        this.clipSize = clipSize;
        this.reloadSpeed = reloadSpeed;
    }

    private void Update()
    {
        if (ammo < clipSize)
        {
            reloadTimeRemaining -= Time.deltaTime;

            if (reloadTimeRemaining <= 0)
            {
                do
                {
                    ammo++;
                    reloadTimeRemaining += 1.0f / reloadSpeed;
                }
                while (reloadTimeRemaining <= 0 && ammo < clipSize);
                onAmmoChanged(ammo, clipSize);
            }
        }
    }

    public void SetAmmo(UInt8 ammo)
    {
        this.ammo = ammo;
        onAmmoChanged(ammo, clipSize);
    }

    public void SetReloadTimeRemaining(float reloadTimeRemaining)
    {
        this.reloadTimeRemaining = reloadTimeRemaining;
    }
}
