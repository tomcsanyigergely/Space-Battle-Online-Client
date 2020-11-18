using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UInt8 = System.Byte;

public class SpaceshipDrawer : MonoBehaviour
{
    [SerializeField] private Material spaceshipMaterial;
    [SerializeField] private GameObject spaceshipModel;
    [SerializeField] private Transform spaceshipTransform;

    [SerializeField] private ParticleSystem explosion;

    private Material clonedSpaceshipMaterial;

    private Color[] spaceshipColors;
    private Color spaceshipColor;

    private void Awake()
    {
        clonedSpaceshipMaterial = new Material(spaceshipMaterial);

        GetComponent<Health>().onHealingChanged += OnHealingChanged;
        GetComponent<Damageable>().onDamaged += OnDamaged;
    }

    private void Start()
    {
        foreach(Renderer renderer in spaceshipModel.GetComponentsInChildren<Renderer>())
        {
            renderer.material = clonedSpaceshipMaterial;
        }
    }

    public void Init(Color[] spaceshipColors, float radius)
    {
        this.spaceshipColors = spaceshipColors;
        spaceshipTransform.localScale = new Vector3(2, 2, 2) * radius;
    }

    public void SetRotation(float angle)
    {
        spaceshipModel.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void Explode()
    {
        explosion.Play();
    }

    private void OnHealingChanged(bool healing)
    {
        // todo: show healing effect
        if (healing)
        {

        }
        else
        {

        }
    }

    private void OnDamaged()
    {
        clonedSpaceshipMaterial.SetFloat("_TimeOfLastDamage", Time.timeSinceLevelLoad);
    }
}
