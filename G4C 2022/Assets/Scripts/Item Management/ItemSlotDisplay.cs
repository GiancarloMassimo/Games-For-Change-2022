using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotDisplay : MonoBehaviour
{
    [SerializeField] GameObject stackDisplay;
    [SerializeField] TMP_Text stackCount;

    Image image;
    Item currentItem;
    TooltipTrigger tooltipTrigger;

    public InventoryDisplay InventoryDisplay { get; set; }
   
    bool placingItem;
    int stack;

    void Awake()
    {
        image = GetComponent<Image>();
        tooltipTrigger = GetComponent<TooltipTrigger>();
        Placable.PlacingItem += SetPlacingItem;
    }

    public void SetItemStack(Item item, int stack)
    {
        currentItem = item;

        tooltipTrigger.Header = currentItem.DisplayName;
        tooltipTrigger.Content = currentItem.Description;

        image.sprite = currentItem.Icon;
        image.SetNativeSize();

        this.stack = stack;
        if (stack > 1)
        {
            stackDisplay.SetActive(true);
            stackCount.text = stack + "";
        }
        else
        {
            stackDisplay.SetActive(false);
        }
    }

    public void ShowHide(bool toggle)
    {
        image.enabled = toggle;
        tooltipTrigger.enabled = toggle;
        stackDisplay.transform.GetChild(0).GetComponent<Image>().enabled = toggle;
        stackDisplay.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().color = new Color(255, 255, 255, toggle ? 1 : 0);
    }

    public void OnClick()
    {
        if (!placingItem)
        {
            Instantiate(currentItem.Placable);
            InventoryDisplay.ItemPlacing = currentItem;
        }
    }

    void SetPlacingItem(bool placingItem)
    {
        this.placingItem = placingItem;
    }

    void OnDisable()
    {
        Placable.PlacingItem += SetPlacingItem;
    }
}
