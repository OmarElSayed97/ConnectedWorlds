using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

namespace Planet
{
    public class PlanetData
    {

        public Planets planetType;
        public Dictionary<Pillars, float> pillarsValues;
        public int pillarsChargedCount;
        public bool isPortalReady;
        public Color planetColor;

        public void ResetPlanet()
        {
            pillarsValues[Pillars.FIRST] = 0;
            pillarsValues[Pillars.SECOND] = 0;
            pillarsValues[Pillars.THIRD] = 0;

            pillarsChargedCount = 0;
            isPortalReady = false;

        }

        public PlanetData(Planets planet)
        {
            planetType = planet;
            pillarsValues = new Dictionary<Pillars, float>();
            for (int i = 0; i < 3; i++)
                pillarsValues.Add((Pillars)i, 0);
            pillarsChargedCount = 0;
            isPortalReady = false;
            switch (planet)
            {
                case Planets.HAPPY_PLANET:
                    planetColor = Color.yellow;
                    break;
                case Planets.SAD_PLANET:
                    planetColor = Color.blue;
                    break;
                case Planets.ANGER_PLANET:
                    planetColor = Color.red;
                    break;
            }

        }


    }
}

