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

    [SerializeField] GameObject gameOver;
    [SerializeField] Image[] gameOverBars;
    [SerializeField] Color achievementUnlockedColor;

    Dictionary<Items, int> itemCounts;
    [HideInInspector]
    public ItemFeeder itemFeeder;
    [HideInInspector]
    public bool floatingIslandTriggered = false, highAltitudeTriggered = false;


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
        if (Input.GetKeyDown(KeyCode.Q) && DayNumber <= 20)
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
        if (DayNumber == 21) dayText.text = 20 + "";
        if (DayNumber <= 20)
        {
            UpdateUnlocks();
            itemFeeder.FeedItems();
        }
        else
        {
            gameOver.SetActive(true);
            if (Renewable())
            {
                gameOverBars[0].color = achievementUnlockedColor;
            }
            if (GoingGreen())
            {
                gameOverBars[1].color = achievementUnlockedColor;
            }
            if (Engineering())
            {
                gameOverBars[2].color = achievementUnlockedColor;
            }
            if (Expansion())
            {
                gameOverBars[3].color = achievementUnlockedColor;
            }
            if (Completionist())
            {
                gameOverBars[4].color = achievementUnlockedColor;
            }


            Time.timeScale = 0;
        }
    }


    public void UpdateUnlocks()
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

        if (!WindTurbineUnlocked && itemCounts[Items.House] > 2)
        {
            itemFeeder.UnlockItem(Items.WindTurbine);
            WindTurbineUnlocked = true;
        }

        if (!UrbanFarmingUnlocked && Population >= 20)
        {
            itemFeeder.UnlockItem(Items.UrbanFarming);
            UrbanFarmingUnlocked = true;
        }

        if (!SkyScraperUnlocked && itemCounts[Items.UrbanFarming] > 0)
        {
            itemFeeder.UnlockItem(Items.SkyScraper);
            SkyScraperUnlocked = true;
        }
        


        if (!ParkUnlocked && floatingIslandTriggered)
        {
            itemFeeder.UnlockItem(Items.Park);
            ParkUnlocked = true;
        }

        if (!BirdHouseUnlocked && itemCounts[Items.Park] > 0)
        {
            itemFeeder.UnlockItem(Items.BirdHouse);
            BirdHouseUnlocked = true;
        }

        if (!RainwaterCollectionUnlocked && itemCounts[Items.BirdHouse] > 0)
        {
            itemFeeder.UnlockItem(Items.RainwaterCollection);
            RainwaterCollectionUnlocked = true;
        }


        if (!ResearchCenterUnlocked && highAltitudeTriggered)
        {
            itemFeeder.UnlockItem(Items.ResearchCenter);
            ResearchCenterUnlocked = true;
        }

        if (!RecyclingPlantUnlocked && itemCounts[Items.ResearchCenter] > 0)
        {
            itemFeeder.UnlockItem(Items.RecyclingPlant);
            RecyclingPlantUnlocked = true;
        }

        if (!CableCarUnlocked && itemCounts[Items.RecyclingPlant] > 0)
        {
            itemFeeder.UnlockItem(Items.CableCar);
            CableCarUnlocked = true;
        }
    }

    bool Renewable()
    {
        return itemCounts[Items.WindTurbine] > 0 && itemCounts[Items.SolarPanel] > 0;
    }

    bool GoingGreen()
    {
        return itemCounts[Items.Park] > 0 && itemCounts[Items.BirdHouse] > 0;
    }

    bool Engineering()
    {
        return itemCounts[Items.SkyScraper] > 0 && itemCounts[Items.CableCar] > 0;
    }

    bool Expansion()
    {
        return Population >= 50;
    }

    bool Completionist()
    {
        foreach (Items item in itemCounts.Keys)
        {
            if (itemCounts[item] == 0) return false; 
        }

        return true;
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
