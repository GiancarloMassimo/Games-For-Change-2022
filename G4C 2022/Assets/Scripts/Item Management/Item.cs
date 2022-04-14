using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class Item : ScriptableObject, IComparable<Item>
{
    [SerializeField] Sprite icon;
    [SerializeField] GameObject placable;
    [SerializeField] string displayName;
    [SerializeField] string description;
    [SerializeField] bool recurring;
    [SerializeField] Items itemType;

    public Sprite Icon { get => icon; }
    public GameObject Placable { get => placable; }
    public string DisplayName { get => displayName; }
    public string Description { get => description; }
    public bool Recurring { get => recurring; }
    public Items ItemType { get => itemType; }

    public int CompareTo(Item other)
    {
        return displayName.CompareTo(other.DisplayName);
    }
}
