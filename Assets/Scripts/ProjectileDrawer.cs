using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UInt8 = System.Byte;

public class ProjectileDrawer : MonoBehaviour
{
    [SerializeField] private GameObject normalProjectile;
    [SerializeField] private GameObject bouncingProjectile;

    private Color[] projectileColors;

    private void Awake()
    {
        GetComponent<Ownable>().onOwnerChanged += OnOwnerChanged;
    }

    public void Init(Color[] projectileColors, float normalProjectileRadius, float bouncingProjectileRadius)
    {
        this.projectileColors = projectileColors;
        normalProjectile.transform.localScale = new Vector3(2, 4, 2) * normalProjectileRadius;
        bouncingProjectile.transform.localScale = new Vector3(2, 2, 2) * bouncingProjectileRadius;
    }

    private void OnOwnerChanged(UInt8 owner)
    {
        // todo: outline using player's color
    }
}
