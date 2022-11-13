using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;
using Managers;
using Planet;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private PlanetManager _planetPlanetManager;
    
    [HideInInspector]
    public Dictionary<Planets, PlanetData> allPlanets;
    [HideInInspector]
    public Dictionary<Planets, float> emotionValues;
    [SerializeField]
    public PlanetData currentPlanet;
    [SerializeField]
    float barsInitialAmount;
    [SerializeField]
    float emotionIncreasingFactor;
    [SerializeField]
    float emotionDecayingFactor;
    [HideInInspector]
    public bool isPlayerTravelling;

    #region UI_References
    [SerializeField]
    public Image[] pillarsIndicator;
    [SerializeField]
    Image[] emotionFillingBars;
    [SerializeField]
    TextMeshProUGUI scoreText;
    float scoreValue;

    #endregion

    public static event Action<Planets> PlayerTravelStarted;
    public static event Action<Planets> PlayerTravelEnded;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        
        _planetPlanetManager = GetComponent<PlanetManager>();
    }

    void Start()
    {
        allPlanets = new Dictionary<Planets, PlanetData>();
        emotionValues = new Dictionary<Planets, float>();
        InitializePlanets();
        InitializeBars();
        currentPlanet = allPlanets[Planets.LOVE_PLANET];
        
    }


    void Update()
    {
        UpdateScore();
        UpdateEmotions();
        if (currentPlanet.pillarsChargedCount >= 3 && !currentPlanet.isPortalReady)
            currentPlanet.isPortalReady = true;
    }


    void UpdateEmotions()
    {
        if (!isPlayerTravelling)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i == (int)currentPlanet.planetType)
                    emotionValues[(Planets)i] += (Time.deltaTime * emotionIncreasingFactor);
                else
                    emotionValues[(Planets)i] -= (Time.deltaTime * emotionDecayingFactor);

                emotionFillingBars[i].fillAmount = (emotionValues[(Planets)i] / 100);
            }

        }
    }

    void UpdateScore()
    {
        if (!isPlayerTravelling)
        {
            scoreValue += (Time.deltaTime * 5);
            scoreText.text = ((int)scoreValue).ToString();
        }

    }

    void InitializePlanets()
    {
        for (int i = 0; i < 3; i++)
        {
            PlanetData newPlanet = new PlanetData((Planets)i);
            allPlanets.Add((Planets)i, newPlanet);
        }

    }
    void InitializeBars()
    {
        for (int i = 0; i < 3; i++)
        {
            emotionValues.Add((Planets)i, barsInitialAmount);
        }
    }

    public PlanetController GetPlanet(Planets planet)
    {
        return _planetPlanetManager.PlanetsDict[planet];
    }


    public void StartPlayerTravel(Planets destinationPlanet)
    {
        if (isPlayerTravelling)
            return;
        currentPlanet.ResetPlanet();
        foreach (var image in pillarsIndicator)
        {
            image.enabled = false;
        }
        currentPlanet = allPlanets[destinationPlanet];
        isPlayerTravelling = true;
        
        PlayerTravelStarted?.Invoke(destinationPlanet);
    }

    public void EndPlayerTravel(Planets destinationPlanet)
    {
        isPlayerTravelling = false;
        PlayerTravelEnded?.Invoke(destinationPlanet);
    }
}
