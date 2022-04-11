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

    Vector2 localOffset;

    void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            localOffset = Vector2.zero;
        }
        else
        {
            localOffset = spriteRenderer.transform.localPosition;
        }
        SetPosition();
    }

    void Start()
    {
        PlacingItem?.Invoke(true);
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
        transform.position = RoundToInt(transform.position);
        transform.position = RestrictToMapBounds(transform.position);
    }

    Vector2 RoundToInt(Vector2 vector)
    {
        return new Vector2(
            (float)Math.Round(vector.x, MidpointRounding.ToEven),
            (float)Math.Round(vector.y, MidpointRounding.ToEven)
            );
    }

    Vector2 RestrictToMapBounds(Vector2 vector)
    {
        const float MapOffsetY = 5.51f;
        const float MapWidth = 38, MapHeight = 21;
        float width = spriteRenderer.sprite.bounds.size.x, height = spriteRenderer.sprite.bounds.size.y;

        return new Vector2(
                Mathf.Clamp(vector.x, -MapWidth / 2 + width / 2 - localOffset.x, 
                                       MapWidth / 2 - width / 2 - localOffset.x),
                Mathf.Clamp(vector.y, -MapHeight / 2 + height / 2 + MapOffsetY - localOffset.y,
                                       MapHeight / 2 - height / 2 + MapOffsetY - localOffset.y)
            );
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
