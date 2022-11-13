using System;
using Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Planet
{
    public class PlanetController : MonoBehaviour
    {
        [SerializeField] private Planets planetType;
        [SerializeField] private PortalManager[] portals;
        [SerializeField] private DestinationPoint[] destinationPoints;
        [SerializeField] public Vector3 startPoint, endPoint;
        private Rigidbody rb;

        public Planets PlanetType => planetType;
        public Rigidbody PlanetBody => rb;
        private void Awake()
        {
            //rb = GetComponent<Rigidbody>();
        }
        private void Start()
        {
            SubscribeToPortals();
            startPoint = transform.position;

        }

        private void SubscribeToPortals()
        {
            foreach (var portal in portals)
            {
                portal.PlayerEntered += PortalOnPlayerEntered;
            }
        }

        private void PortalOnPlayerEntered(Planets obj)
        {
            GameManager.Instance.StartPlayerTravel(obj);
        }

        public DestinationPoint GetRandomDestinationPoint()
        {
            return destinationPoints[Random.Range(0, destinationPoints.Length)];
        }


        [Button]
        private void SetRefs()
        {
            portals = GetComponentsInChildren<PortalManager>(true);
            destinationPoints = GetComponentsInChildren<DestinationPoint>(true);
        }
    }
}
