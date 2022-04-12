using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemFeeder : MonoBehaviour
{
    [SerializeField] Item[] items;
    [SerializeField] Item plank, ladder, leftStair, rightStair;

    [Header("Building Item Ratio Distribution")]
    [SerializeField] float plankRatio, ladderRatio, stairRatio;

    float plankRate, ladderRate, stairRate;

    Queue<Item> unlockedItems;
    Queue<Item> recurringItems;

    public Inventory Inventory {get; set;}

    void Start()
    {
        unlockedItems = new Queue<Item>();
        recurringItems = new Queue<Item>();

        ConvertItemRatiosToPercentages();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            FeedItems();
        } 
    }

    public void FeedItems()
    {
        while (unlockedItems.Count > 0 && Inventory.ItemsInInventory < Inventory.MaxItems)
        { 
            Item item = unlockedItems.Dequeue();
            Inventory.Add(item);
            if (item.Recurring)
            {
                recurringItems.Enqueue(item);
            }
        }

        int buildingItemsToAdd = Random.Range(3, 6);
        while (buildingItemsToAdd > 0)
        {
            AddBuildingItem();
            buildingItemsToAdd--;
        }

        int recurringItemsToAdd = Inventory.MaxItems - buildingItemsToAdd;
        while (recurringItemsToAdd > 0)
        {
            if (recurringItems.Count == 0)
            {
                AddBuildingItem();
            }
            else
            {
                recurringItems.Enqueue(recurringItems.Dequeue());
            }
            recurringItemsToAdd--;
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
        stairRate = stairRatio / total;
    }

}
