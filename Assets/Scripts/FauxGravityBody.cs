using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityBody : MonoBehaviour
{
    [SerializeField]
    FauxGravityAttractor attractor;
    Transform playerTransform;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
        playerTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        attractor.Attract(playerTransform);
    }
}
