using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableWithAtLeastOneNeccessaryOverlapMet : Placable
{
    protected override bool CanPlace()
    {
        return CheckAtLeastOneNeccessaryOverlapMet() && CheckInvalidOverlaps();
    }

    bool CheckAtLeastOneNeccessaryOverlapMet()
    {
        foreach (CheckPlacableOverlapBox overlap in mustOverlap)
        {
            if (overlap.CheckOverlap())
            {
                return true;
            }
        }
        return false;
    }
}
