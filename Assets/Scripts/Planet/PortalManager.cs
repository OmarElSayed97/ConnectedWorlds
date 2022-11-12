using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;
using DG.Tweening;
using UnityEngine.UI;

public class PortalManager : MonoBehaviour
{

    [SerializeField]
    Planets destinationPlanet;
    [SerializeField]
    Transform destinationPoint;

    GameManager gameManager;

    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !gameManager.isPlayerTravelling && gameManager.currentPlanet.isPortalReady)
        {
            Debug.Log("Entered Portal");
            gameManager.currentPlanet.ResetPlanet();
            foreach (Image image in gameManager.pillarsIndicator)
            {
                image.enabled = false;
            }
            gameManager.currentPlanet = gameManager.allPlanets[destinationPlanet];
            gameManager.isPlayerTravelling = true;
            player.GetComponent<CapsuleCollider>().isTrigger = true;
            player.DOMove(destinationPoint.position, 3f).OnComplete(Complete);
            void Complete()
            {
                player.GetComponent<CapsuleCollider>().isTrigger = false;
                gameManager.isPlayerTravelling = false;
            }
        }

    }
}
