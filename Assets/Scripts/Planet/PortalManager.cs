using System;
using DG.Tweening;
using Enum;
using UnityEngine;
using UnityEngine.UI;

namespace Planet
{
    public class PortalManager : MonoBehaviour
    {

        [SerializeField] private Planets destinationPlanet;
        // [SerializeField] private Transform destinationPoint;

        GameManager gameManager;

        // Transform player;

        public event Action<Planets> PlayerEntered;
        // Start is called before the first frame update
        void Start()
        {
            gameManager = GameManager.Instance;
            // player = GameObject.FindGameObjectWithTag("Player").transform;

        }



        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !gameManager.isPlayerTravelling && gameManager.currentPlanet.isPortalReady)
            {
                OnPlayerEntered();
            }

        }

        protected virtual void OnPlayerEntered()
        {
            Debug.Log("Entered Portal");
            PlayerEntered?.Invoke(destinationPlanet);
        }
    }
}
