using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance { get; private set; }


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

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
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
}
