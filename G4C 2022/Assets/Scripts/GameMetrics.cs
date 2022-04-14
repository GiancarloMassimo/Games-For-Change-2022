using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMetrics : MonoBehaviour
{
    public static GameMetrics Instance;

    [SerializeField] TMP_Text dayText, popText;
    [SerializeField] GameObject pauseMenu,
        Solar, Wind, House, Skyscrape, Recycle, Research, Park, Farm, Bird, Rain, Car;

    Dictionary<Items, int> itemCounts;
    ItemFeeder itemFeeder;

    public int DayNumber { get; set; }
    public int Population { get; set; }

    bool SolarPanelUnlocked,
        WindTurbineUnlocked,
        HouseUnlocked,
        SkyScraperUnlocked,
        RecyclingPlantUnlocked,
        ResearchCenterUnlocked,
        ParkUnlocked,
        UrbanFarmingUnlocked,
        BirdHouseUnlocked,
        RainwaterCollectionUnlocked,
        CableCarUnlocked;


    private void OnEnable()
    {
        WorldItem.itemSpawn += ItemPlaced;
    }

    private void OnDisable()
    {
        WorldItem.itemSpawn -= ItemPlaced;
    }

    private void Awake()
    {
        DayNumber = 1;
        Population = 1;
    }

    void Start()
    {
        Instance = this;

        itemCounts = new Dictionary<Items, int>();

        itemFeeder = FindObjectOfType<ItemFeeder>();

        foreach (Items i in Enum.GetValues(typeof(Items)))
        {
            itemCounts.Add(i, 0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Time.timeScale == 0)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                pauseMenu.SetActive(true);
                Solar.SetActive(SolarPanelUnlocked);
                Wind.SetActive(WindTurbineUnlocked);
                House.SetActive(HouseUnlocked);
                Skyscrape.SetActive(SkyScraperUnlocked);
                Recycle.SetActive(RecyclingPlantUnlocked);
                Research.SetActive(ResearchCenterUnlocked);
                Park.SetActive(ParkUnlocked);
                Farm.SetActive(UrbanFarmingUnlocked);
                Bird.SetActive(BirdHouseUnlocked);
                Rain.SetActive(RainwaterCollectionUnlocked);
                Car.SetActive(CableCarUnlocked);
                Time.timeScale = 0;
            }
        }
    }

    void ItemPlaced(Items item)
    {
        itemCounts[item]++;

        if (item == Items.House) Population += 3;
        if (item == Items.SkyScraper) Population += 5;

        popText.text = (Population < 10 ? "0" : "") + Population;

        UpdateUnlocks();
    }

    public void NextDay()
    {
        DayNumber++;
        dayText.text = "Day " + (DayNumber < 10 ? "0" : "") + DayNumber;
        UpdateUnlocks();
    }


    void UpdateUnlocks()
    {
        if (!SolarPanelUnlocked && DayNumber == 2)
        {
            itemFeeder.UnlockItem(Items.SolarPanel);
            SolarPanelUnlocked = true;
        }

        if (!HouseUnlocked && itemCounts[Items.SolarPanel] > 0)
        {
            itemFeeder.UnlockItem(Items.House);
            HouseUnlocked = true;
        }
    }
}

public enum Items {
    SolarPanel,
    WindTurbine,
    House,
    SkyScraper,
    RecyclingPlant,
    ResearchCenter,
    Park,
    UrbanFarming,
    BirdHouse,
    RainwaterCollection,
    CableCar
}
