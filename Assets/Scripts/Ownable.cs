using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UInt8 = System.Byte;

public class Ownable : MonoBehaviour
{
    public UnityAction<UInt8> onOwnerChanged;

    public UInt8 Owner { get; private set; }

    public void SetOwner(UInt8 owner)
    {
        Owner = owner;
        onOwnerChanged?.Invoke(Owner);
    }
}
