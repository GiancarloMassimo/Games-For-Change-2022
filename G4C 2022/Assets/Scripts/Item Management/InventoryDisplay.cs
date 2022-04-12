using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] GameObject itemSlot;
    [SerializeField] Image panelImage;

    Inventory inventory;
    ItemFeeder itemFeeder;
    ItemSlotDisplay[] itemSlots;

    public Item ItemPlacing { get; set; }

    void Awake()
    {
        inventory = new Inventory();
        Inventory.InventoryUpdated += UpdateDisplay;
        Placable.PlacingItem += ShowHideInventory;
        CreateItemSlots();
    }

    void Start()
    {
        itemFeeder = GetComponent<ItemFeeder>();
        itemFeeder.Inventory = inventory;
        itemFeeder.FeedItems();
        ShowHideInventory(true);
        Invoke(nameof(ShowInventoryAfterEntryAnimation), 5f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && ItemPlacing != null)
        {
            ItemPlacing = null;
            Destroy(FindObjectOfType<Placable>().gameObject);
        }
    }

    public void UpdateDisplay()
    {
        int itemIndex = 0;
        panelImage.enabled = inventory.ItemsInInventory > 0;
        foreach (Item item in inventory.Items.Keys)
        {
            itemSlots[itemIndex].gameObject.SetActive(true);
            itemSlots[itemIndex].SetItemStack(item, inventory.Items[item]);
            itemIndex++;
        }

        for (int i = inventory.Items.Count; i < itemSlots.Length; i++)
        {
            itemSlots[i].gameObject.SetActive(false);
        }
    }

    void CreateItemSlots()
    {
        itemSlots = new ItemSlotDisplay[inventory.MaxItems];
        for (int i = 0; i < itemSlots.Length; i++)
        {
            GameObject newItemSlot = Instantiate(itemSlot, transform);
            ItemSlotDisplay newItemSlotDisplay = newItemSlot.GetComponent<ItemSlotDisplay>();
            itemSlots[i] = newItemSlotDisplay;
            newItemSlot.SetActive(false);
            newItemSlotDisplay.InventoryDisplay = this;
        }
    }

    void ShowHideInventory (bool placingItem)
    {
        bool toggle = !placingItem;
        panelImage.enabled = toggle;

        foreach (ItemSlotDisplay itemSlotDisplay in itemSlots)
        {
            itemSlotDisplay.ShowHide(toggle);
        }

        // I know this is a side-effect of the method, but it's late so I don't care!
        if (!placingItem && ItemPlacing != null)
        {
            inventory.Remove(ItemPlacing);
            ItemPlacing = null;
        }
    }

    void ShowInventoryAfterEntryAnimation()
    {
        ShowHideInventory(false);
    }

    void OnDisable()
    {
        Inventory.InventoryUpdated -= UpdateDisplay;
        Placable.PlacingItem -= ShowHideInventory;
    }
}
