using System;
using System.Collections.Generic;
using Enum;
using Planet;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class PlanetManager : MonoBehaviour
    {
        [SerializeField] private PlanetController[] planetControllers;
        [SerializeField] private Dictionary<Planets, PlanetController> _planetsDict;

        public Dictionary<Planets, PlanetController> PlanetsDict => _planetsDict;

        public PlanetController[] PlanetControllers => planetControllers;

        private void Awake()
        {
            InitDictionary();
        }

        private void InitDictionary()
        {
            _planetsDict = new Dictionary<Planets, PlanetController>();

            foreach (var controller in planetControllers)
            {
                _planetsDict.Add(controller.PlanetType, controller);
            }
        }

        [Button]
        private void SetRefs()
        {
            planetControllers = GetComponentsInChildren<PlanetController>(true);
        }
    }
}
