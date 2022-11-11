using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;
using DG.Tweening;

public class PortalManager : MonoBehaviour
{
    [SerializeField]
    Portals portalType;

    [SerializeField]
    Planets hostPlanet;
    [SerializeField]
    Transform destinationPoint;

    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<CapsuleCollider>().isTrigger = true;
            player.DOMove(destinationPoint.position, 3f).OnComplete(Complete);
            void Complete()
            {
                player.GetComponent<CapsuleCollider>().isTrigger = false;
            }
        }

    }
}
