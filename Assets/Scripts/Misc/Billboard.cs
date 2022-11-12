using System;
using Unity.Mathematics;
using UnityEngine;

namespace Misc
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private bool mainCamera = true;
        [SerializeField] private Camera camera;

        private Transform _cameraTransform;
        private void Awake()
        {
            if (mainCamera)
                camera = Camera.main;
            _cameraTransform = camera.transform;
        }
        
        private void Update()
        {
            UpdateTransform();
        }
        
        private void UpdateTransform()
        {
            transform.rotation = quaternion.LookRotation(_cameraTransform.forward, _cameraTransform.up);
        }
    }
}
