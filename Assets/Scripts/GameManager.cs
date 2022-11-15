using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;
using Managers;
using Planet;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private PlanetManager _planetPlanetManager;

    [HideInInspector]
    public Dictionary<Planets, PlanetData> allPlanetsData;
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

    [SerializeField, PropertyRange(0, 100)] private float planetNearDeathThreshold;

    [HideInInspector]
    public bool isPlayerTravelling;

    #region UI_References
    [SerializeField]
    public Image[] pillarsIndicator;
    [SerializeField]
    UIRadialSlider[] emotionsRadialBars;
    [SerializeField]
    TextMeshProUGUI scoreText;
    float scoreValue;

    //Sun Movement
    [SerializeField]
    Transform sun;
    Material sunMaterial;
    Vector3 sunPosAtArrival;
    Color sunColorbeforeArrival;
    float curEmotionAtArrival;

    #endregion

    public static event Action<Planets> PlayerTravelStarted;
    public static event Action<Planets> PlayerTravelEnded;

    public static event Action<Planets> PlanetNearDeath;
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
        allPlanetsData = new Dictionary<Planets, PlanetData>();
        emotionValues = new Dictionary<Planets, float>();
        InitializePlanets();
        InitializeBars();
        currentPlanet = allPlanetsData[Planets.HAPPY_PLANET];
        sunMaterial = sun.GetComponent<Renderer>().material;
        sunColorbeforeArrival = Color.white;

    }


    void Update()
    {
        UpdateScore();
        UpdateEmotions();
        MoveSun();
        if (currentPlanet.pillarsChargedCount >= 3 && !currentPlanet.isPortalReady)
            currentPlanet.isPortalReady = true;
    }


    void UpdateEmotions()
    {
        if (!isPlayerTravelling)
        {
            for (int i = 0; i < 3; i++)
            {
                var planetPreviousValue = emotionValues[(Planets)i];
                if (i == (int)currentPlanet.planetType)
                {
                    if (emotionValues[(Planets)i] <= 100)
                        emotionValues[(Planets)i] += (Time.deltaTime * emotionIncreasingFactor);
                }

                else
                {
                    if (emotionValues[(Planets)i] >= 0)
                        emotionValues[(Planets)i] -= (Time.deltaTime * emotionDecayingFactor);
                }


                emotionsRadialBars[i].Slider = (emotionValues[(Planets)i] * 0.005f) + 0.5f;

                if (planetPreviousValue > planetNearDeathThreshold &&
                    emotionValues[(Planets)i] < planetNearDeathThreshold)
                {
                    PlanetNearDeath?.Invoke((Planets)i);
                }
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
            PlanetData newPlanet;
            if (i == 0)
                newPlanet = new PlanetData((Planets)i, 0);
            else
                newPlanet = new PlanetData((Planets)i, 70f);
            allPlanetsData.Add((Planets)i, newPlanet);
        }

    }
    void InitializeBars()
    {
        for (int i = 0; i < 3; i++)
        {
            emotionValues.Add((Planets)i, allPlanetsData[(Planets)i].initialValue);
            emotionsRadialBars[i].Slider = (allPlanetsData[(Planets)i].initialValue * 0.01f) / 2 + 0.5f;

        }
    }
    void MoveSun()
    {
        float lerpingFactor = (emotionValues[currentPlanet.planetType] - curEmotionAtArrival) / (100 - curEmotionAtArrival);
        sun.position = Vector3.Lerp(sunPosAtArrival, GetPlanet(currentPlanet.planetType).startPoint, lerpingFactor);
        sunMaterial.color = Color.Lerp(sunColorbeforeArrival, allPlanetsData[currentPlanet.planetType].planetColor, lerpingFactor);
        sunMaterial.SetColor("_EmissionColor", Color.Lerp(sunColorbeforeArrival, allPlanetsData[currentPlanet.planetType].planetColor, lerpingFactor) * 2);
        sunMaterial.EnableKeyword("_EMISSION");

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
            image.transform.DOScale(Vector3.zero, 1);
            image.enabled = false;
        }
        sunColorbeforeArrival = allPlanetsData[currentPlanet.planetType].planetColor;
        currentPlanet = allPlanetsData[destinationPlanet];
        isPlayerTravelling = true;

        PlayerTravelStarted?.Invoke(destinationPlanet);
    }

    public void EndPlayerTravel(Planets destinationPlanet)
    {
        isPlayerTravelling = false;
        sunPosAtArrival = sun.position;
        curEmotionAtArrival = emotionValues[destinationPlanet];
        PlayerTravelEnded?.Invoke(destinationPlanet);
    }
}
