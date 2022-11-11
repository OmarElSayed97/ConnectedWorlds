using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheckerSource;
    [SerializeField] private float gravityForce = 10f;
    [SerializeField] private float detectionRadius = 4f;
    [SerializeField] private float groundDetectionDistance = 2f;

    [SerializeField] private bool grounded;

    private Rigidbody _rb;

    private FauxGravityBody _fauxBody;

    private Ray _ray;
    // private RaycastHit _hit;

    private Vector2 _input;
    private Vector3 _projectedForward;
    private Vector3 _currentNormal;

    private Collider[] _planetsColliders;

    [SerializeField] private Transform _currentPlanet;


    private void Awake()
    {
        _fauxBody = GetComponent<FauxGravityBody>();
        _rb = GetComponent<Rigidbody>();
        _planetsColliders = new Collider[10];
    }

    private void Start()
    {
        _ray = new Ray();
        _input = new Vector2();
        _projectedForward = new Vector3();
        _currentNormal = Vector3.up;
    }

    private bool PlanetDetection()
    {
        if (Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, _planetsColliders, groundMask) > 0)
        {
            var minDistance = Mathf.Infinity;
            foreach (var t in _planetsColliders)
            {
                if (!t)
                    continue;

                var squareDistance = (t.transform.position - transform.position).sqrMagnitude;
                if (!(squareDistance < minDistance)) continue;
                
                minDistance = squareDistance;
                _currentPlanet = t.transform;
            }

            return true;
        }

        _currentPlanet = null; 
        return false;
    }

    private void FixedUpdate()
    {
        // _fauxBody.Attract();

        _input.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _input.Normalize();

        var trans = transform;
        
        _ray.origin = groundCheckerSource.position;
        _ray.direction = trans.position - _ray.origin;

        if (Physics.Raycast(_ray, groundDetectionDistance, groundMask) && _currentPlanet)
        {
            grounded = true;
            _currentNormal = (trans.position - _currentPlanet.position).normalized;
            _projectedForward = Vector3.ProjectOnPlane(trans.forward, _currentNormal);
            _rb.rotation = Quaternion.LookRotation(_projectedForward, _currentNormal);
            _rb.velocity = _projectedForward * (_input.y * speed) + (-_currentNormal * gravityForce);
            _rb.angularVelocity = _input.x * rotationSpeed * _currentNormal;
        }
        else 
        {
            PlanetDetection();
            grounded = false;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, _projectedForward);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(_ray);
    }
}
