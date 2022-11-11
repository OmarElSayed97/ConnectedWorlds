using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheckerSource;

    private Vector3 _movDir;
    private Rigidbody _rb;

    private FauxGravityBody _fauxBody;

    private Ray _ray;
    private RaycastHit _hit;

    private Vector2 _input;
    private Vector3 _projectedForward;
    private void Awake()
    {
        _fauxBody = GetComponent<FauxGravityBody>();
        _rb = GetComponent<Rigidbody>();
        
    }
    
    private void Start()
    {
        _ray = new Ray();
        _input = new Vector2();
        _movDir = new Vector3();
        _projectedForward = new Vector3();
    }

    private void FixedUpdate()
    {
        // _fauxBody.Attract();
        
        _input.Set(Input.GetAxisRaw("Horizontal"),  Input.GetAxisRaw("Vertical")); 
        _input.Normalize();
        
        var trans = transform;

        _ray.origin = groundCheckerSource.position;
        _ray.direction = -_ray.origin;

        if (Physics.Raycast(_ray, out _hit, 10, groundMask))
        {
            _projectedForward = Vector3.ProjectOnPlane(trans.forward, _hit.normal);
            _rb.rotation = Quaternion.LookRotation(_projectedForward, _hit.normal);
            _rb.velocity = _projectedForward * (_input.y * speed);
        }
        
        
        _rb.angularVelocity = -_input.x * rotationSpeed * Vector3.up;
        
    }

    private void CheckGround()
    {
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, _projectedForward);
    }
}
