using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemFeeder : MonoBehaviour
{
    [SerializeField] Item[] items;
    [SerializeField] Item plank, ladder, leftStair, rightStair;
    [SerializeField] Animator popup;
    [SerializeField] TMP_Text popupHeader, popupText;

    [Header("Building Item Ratio Distribution")]
    [SerializeField] float plankRatio, ladderRatio, stairRatio;
    [SerializeField] int houseToEnergyRatio;

    float plankRate, ladderRate;

    Queue<Item> unlockedItems;
    List<Item> recurringHouse;
    List<Item> recurringEnergy;

    public Inventory Inventory {get; set;}

    private void Awake()
    {
        unlockedItems = new Queue<Item>();
        recurringHouse = new List<Item>();
        recurringEnergy = new List<Item>();
        ConvertItemRatiosToPercentages();
    }

    public void FeedItems()
    {
        List<Item> recurring = new List<Item>();
        while (unlockedItems.Count > 0 && Inventory.ItemsInInventory < Inventory.MaxItems)
        {
            Item item = unlockedItems.Dequeue();
            Inventory.Add(item);
            if (item.Recurring)
            {
                recurring.Add(item);
            }
        }

        int buildingItemsToAdd = Random.Range(2, 4);
        int n = buildingItemsToAdd;
        while (buildingItemsToAdd > 0)
        {
            AddBuildingItem();
            buildingItemsToAdd--;
        }

        int recurringItemsToAdd = Inventory.MaxItems - n;
        while (recurringItemsToAdd > 0)
        {
            if (recurringHouse.Count == 0)
            {
                AddBuildingItem();
            }
            else
            {
                if (recurringHouse.Count == 0)
                {
                    Inventory.Add(recurringEnergy[0]);
                    continue;
                }
                else
                {
                    if (Random.Range(0f, 1f) < 1f/(houseToEnergyRatio + 1f))
                    {
                        Inventory.Add(recurringEnergy[Random.Range(0, recurringEnergy.Count)]);
                    }
                    else
                    {
                        Inventory.Add(recurringHouse[Random.Range(0, recurringHouse.Count)]);
                    }
                }
            }
            recurringItemsToAdd--;
        }

        foreach (Item item in recurring)
        {
            if (item.ItemType == Items.House || item.ItemType == Items.SkyScraper)
                recurringHouse.Add(item);
            else
                recurringEnergy.Add(item);
        }
    }

    public void AddBuildingItem()
    {
        float p = Random.Range(0f, 1f);

        if (p < plankRate)
        {
            Inventory.Add(plank);
        }
        else if (p < plankRate + ladderRate)
        {
            Inventory.Add(ladder);
        }
        else
        {
            Inventory.Add(Random.Range(0f, 1f) < 0.5f ? leftStair : rightStair);
        }
    }

    void ConvertItemRatiosToPercentages()
    {
        float total = (plankRatio + ladderRatio + stairRatio);
        plankRate = plankRatio / total;
        ladderRate = ladderRatio / total;
    }

    public void UnlockItem(Items type)
    {
        foreach (Item i in items)
        {
            if (i.ItemType == type)
            {
                popupHeader.text = i.DisplayName;
                popupText.text = i.Description;
                popup.SetTrigger("show");
                unlockedItems.Enqueue(i);
            }
        }
    }
}
