using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedDrawer : MonoBehaviour
{
    [SerializeField] private GameObject attackedMarker;

    public bool Attacked { get; private set; }

    public void SetAttacked(bool attacked)
    {
        Attacked = attacked;
        attackedMarker.SetActive(Attacked);
    }
}
