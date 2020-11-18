using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UInt8 = System.Byte;

public class AmmoDrawer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;

    private void Awake()
    {
        GetComponent<Ammo>().onAmmoChanged += OnAmmoChanged;
    }

    private void OnAmmoChanged(UInt8 ammo, UInt8 clipSize)
    {
        ammoText.text = Convert.ToString(ammo);
        if (ammo == clipSize)
        {
            ammoText.color = Color.green;
        }
        else if (ammo == 0)
        {
            ammoText.color = Color.red;
        }
        else
        {
            ammoText.color = Color.white;
        }
    }
}
