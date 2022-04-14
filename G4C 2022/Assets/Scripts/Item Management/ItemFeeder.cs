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

    float plankRate, ladderRate;

    Queue<Item> unlockedItems;
    Queue<Item> recurringItems;

    bool doubledUpOnHouses = false;

    public Inventory Inventory {get; set;}

    private void Awake()
    {
        unlockedItems = new Queue<Item>();
        recurringItems = new Queue<Item>();
        ConvertItemRatiosToPercentages();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameMetrics.Instance.NextDay();
            FeedItems();
        } 
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
            if (recurringItems.Count == 0)
            {
                AddBuildingItem();
            }
            else
            {
                Item i = recurringItems.Peek();
                Inventory.Add(i);
                if (i.ItemType == Items.House && !doubledUpOnHouses)
                {
                    doubledUpOnHouses = true;
                    recurringItems.Enqueue(i);
                }
                else
                {
                    recurringItems.Enqueue(recurringItems.Dequeue());
                }
            }
            recurringItemsToAdd--;
        }

        foreach (Item item in recurring)
        {
            recurringItems.Enqueue(item);
        }
    }

    void AddBuildingItem()
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
