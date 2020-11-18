using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UInt8 = System.Byte;

public class ControlPointDrawer : MonoBehaviour
{
    [SerializeField] private GameObject controlPointArea;

    private Renderer controlPointAreaRenderer;

    private Color[] controlPointAreaColors;

    private void Awake()
    {
        controlPointAreaRenderer = controlPointArea.GetComponent<Renderer>();

        GetComponent<Ownable>().onOwnerChanged += OnOwnerChanged;
        GetComponent<ControlPoint>().onContestingChanged += OnContestingChanged;
    }

    public void Init(Color[] controlPointAreaColors, float radius)
    {
        this.controlPointAreaColors = controlPointAreaColors;
        controlPointArea.transform.localScale = new Vector3(2 * radius, 2 * radius, Config.CONTROL_AREA_Z_SCALE);
    }

    private void OnOwnerChanged(UInt8 owner)
    {
        controlPointAreaRenderer.material.color = controlPointAreaColors[owner];
    }

    private void OnContestingChanged(bool[] isContesting)
    {

    }
}
