using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyRangeController : MonoBehaviour
{
    [SerializeField] float startingDiameter;

    SpriteRenderer sr;
    float visibleOpacity;

    void OnEnable()
    {
        Placable.PlacingItem += ToggleVisibility;
    }

    void OnDisable()
    {
        Placable.PlacingItem -= ToggleVisibility;
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        visibleOpacity = sr.color.a;
        ToggleVisibility(false);
        transform.localScale = new Vector2(startingDiameter, startingDiameter);
    }

    void ToggleVisibility(bool toggle)
    {
        Placable placable = FindObjectOfType<Placable>();
        if (placable != null && !placable.MakeEnergyRangesVisible)
        {
            toggle = false;
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, toggle ? visibleOpacity : 0f);
    }
}
