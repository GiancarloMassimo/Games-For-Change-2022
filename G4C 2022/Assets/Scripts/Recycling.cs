using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycling : InteractionTrigger
{
    int lastTurnRecycled = 0;

    private void Update()
    {
        if (touching &&
            (GameMetrics.Instance.itemFeeder.Inventory.ItemsInInventory >= GameMetrics.Instance.itemFeeder.Inventory.MaxItems 
            || lastTurnRecycled == GameMetrics.Instance.DayNumber))
        {
            indicator.ToggleIndicator(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && indicator.Showing && touching
            && GameMetrics.Instance.itemFeeder.Inventory.ItemsInInventory < GameMetrics.Instance.itemFeeder.Inventory.MaxItems)
        {
            Action();
            lastTurnRecycled = GameMetrics.Instance.DayNumber;
            indicator.ToggleIndicator(false);
        }
    }

    void Action()
    {
        GameMetrics.Instance.itemFeeder.AddBuildingItem();
    }
}
