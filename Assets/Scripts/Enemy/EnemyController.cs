using System;
using Misc;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(AutoAlign))]
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float currentRotationAngle;
        [SerializeField, OnValueChanged(nameof(OnMovementSpeedChanged))] private float movementSpeed = 2f;
        [SerializeField] private float rotationSpeed = 20f;

        [SerializeField, ReadOnly] private Vector3 velocity;
        
        private Transform _planet;

        private AutoAlign _aligner;

        private Vector3 _movementDirection;
        
        private void Awake()
        {
            _aligner = GetComponent<AutoAlign>();
            velocity = Vector3.zero;
        }

        private void Start()
        {
            OnMovementSpeedChanged();
        }

        private void OnMovementSpeedChanged()
        {
            _movementDirection = Vector3.forward * (movementSpeed);
        }
        
        private void Update()
        {
            _aligner.FixOrientation();

            var deltaTime = Time.deltaTime;
            var trans = transform;
            velocity = _movementDirection;

            var deltaRotation = rotationSpeed * deltaTime;
            var directedDeltaRotation = currentRotationAngle > 0 ? 1f : -1f;
            
            if (Mathf.Abs(currentRotationAngle) - deltaRotation < 0)
            {
                deltaRotation = currentRotationAngle;
                currentRotationAngle = 0f;
            }
            
            if (currentRotationAngle != 0)
                currentRotationAngle -= directedDeltaRotation;
            
            directedDeltaRotation *= deltaRotation;
            
            trans.Rotate(Vector3.up, directedDeltaRotation, Space.Self);
            
            trans.Translate(velocity * deltaTime, Space.Self);
        }

    }
}