using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public static event Action<Items> itemSpawn;

    [SerializeField] Items itemType;

    void Start()
    {
        itemSpawn.Invoke(itemType);
    }
}
