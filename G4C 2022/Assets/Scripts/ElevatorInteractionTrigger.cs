using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorInteractionTrigger : InteractionTrigger
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && indicator.Showing && touching)
        {
            Action();
            indicator.ToggleIndicator(false);
        }
    }

    void Action()
    {
        GameMetrics.Instance.NextDay();
        if (GameMetrics.Instance.DayNumber == 20)
        {
            message = "End Game";
        }
    }
}
