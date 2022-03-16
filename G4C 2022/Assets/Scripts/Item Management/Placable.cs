using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Placable : MonoBehaviour
{
    public static event Action<bool> PlacingItem;

    [SerializeField] bool makeEnergyRangesVisible;

    public bool MakeEnergyRangesVisible { get => makeEnergyRangesVisible; }

    [SerializeField] GameObject obejctToPlace;
    [SerializeField] Color invalidColor, validColor;
    
    [SerializeField] protected CheckPlacableOverlapBox[] mustOverlap;
    [SerializeField] protected CheckPlacableOverlapBox[] cannotOverlap;

    [SerializeField] SpriteRenderer spriteRenderer;
    bool canPlace;

    void Awake()
    {
        SetPosition();
    }

    void Start()
    {
        PlacingItem?.Invoke(true);
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        SetPosition();
        canPlace = CanPlace();
        SetColor();

        if (Input.GetMouseButton(0) && canPlace)
        {
            PlaceObject();
        }

    }

    void SetPosition()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y));
    }

    void SetColor()
    {
        spriteRenderer.color = canPlace ? validColor : invalidColor;
    }

    protected virtual bool CanPlace()
    {
        return CheckNecessaryOverlaps()  && CheckInvalidOverlaps();
    }

    protected bool CheckNecessaryOverlaps()
    {
        foreach(CheckPlacableOverlapBox overlap in mustOverlap)
        {
            if (!overlap.CheckOverlap())
            {
                return false;
            }
        }
        return true;
    }

    protected bool CheckInvalidOverlaps()
    {
        foreach (CheckPlacableOverlapBox overlap in cannotOverlap)
        {
            if (overlap.CheckOverlap())
            {
                return false;
            }
        }
        return true;
    }

    void PlaceObject()
    {
        Instantiate(obejctToPlace, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        PlacingItem?.Invoke(false);
    }
}
