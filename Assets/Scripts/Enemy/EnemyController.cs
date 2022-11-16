using System;
using Misc;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    [RequireComponent(typeof(AutoAlign))]
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float currentRotationAngle;
        [SerializeField, OnValueChanged(nameof(OnMovementSpeedChanged))] private float movementSpeed = 5f;
        [SerializeField] private float rotationSpeed = 70f;

        [SerializeField] private AnimationCurve movementCurve;
        [SerializeField] private AnimationCurve rotationCurve;

        [SerializeField, ReadOnly] private Vector3 velocity;

        [SerializeField, TitleGroup("Patrol"), MinMaxSlider(0, 60, true)] 
        private Vector2 angleSwitchTimeRange = new Vector2(3f, 7f);

        [SerializeField, TitleGroup("ObstacleDetection")]
        private float detectionDistance = 5f;

        [SerializeField, TitleGroup("ObstacleDetection")]
        private float detectionSphereRadius = 0.7f;
        
        [SerializeField, TitleGroup("ObstacleDetection")]
        private Vector3 detectionSourceOffset = new Vector3(0, 1, 0);
        
        [SerializeField,  TitleGroup("ObstacleDetection")] 
        private float rotationCorrectionIncrements = 5f;
        
        [SerializeField, TitleGroup("ObstacleDetection")]
        private LayerMask obstacleDetectionMask;
        

        private Collider[] _colliders;
        private Ray _ray = new Ray();
        
        private Transform _planet;

        private AutoAlign _aligner;

        private Vector3 _movementDirection;

        private Collider[] _detectionColliders;

        private Transform _trans;

        [SerializeField, TitleGroup("Patrol"), ReadOnly] private float _switchRotationTimer;
        
        private void Awake()
        {
            _aligner = GetComponent<AutoAlign>();
            velocity = Vector3.zero;
            _trans = transform;
        }

        private void Start()
        {
            OnMovementSpeedChanged();
            _detectionColliders = new Collider[3];
            _ray = new Ray();
        }

        private void OnMovementSpeedChanged()
        {
            _movementDirection = Vector3.forward * (movementSpeed);
        }
        
        private void Update()
        {
            _aligner.FixOrientation();
            RandomizeRotation();
            DetectObstacles();
            HandleMovement();
        }

        private void HandleMovement()
        {
            var deltaTime = Time.deltaTime;
            velocity = _movementDirection;

            var deltaRotation = rotationSpeed * rotationCurve.Evaluate(Mathf.Abs(currentRotationAngle) / 90) * deltaTime;
            var directedDeltaRotation = currentRotationAngle > 0 ? 1f : -1f;
            
            if (Mathf.Abs(currentRotationAngle) - deltaRotation < 0)
            {
                deltaRotation = currentRotationAngle;
                currentRotationAngle = 0f;
            }
            
            if (currentRotationAngle != 0)
                currentRotationAngle -= directedDeltaRotation;
            
            directedDeltaRotation *= deltaRotation;
            
            _trans.Rotate(Vector3.up, directedDeltaRotation, Space.Self);

            velocity *= movementCurve.Evaluate(Mathf.Abs(currentRotationAngle) / 90);
            _trans.Translate(velocity * deltaTime, Space.Self);
        }

        private void DetectObstacles()
        {
            var trans = transform;
            _ray.origin = trans.position + detectionSourceOffset;
            _ray.direction = Quaternion.AngleAxis(currentRotationAngle, trans.up) * trans.forward;

            if (Physics.SphereCast(_ray, detectionSphereRadius, detectionDistance, obstacleDetectionMask))
            {
                if (currentRotationAngle == 0)
                {
                    currentRotationAngle += Random.Range(0, 4) > 1 ? rotationCorrectionIncrements : -rotationCorrectionIncrements;
                }
                else
                {
                    currentRotationAngle += currentRotationAngle > 0 ? rotationCorrectionIncrements : -rotationCorrectionIncrements;
                }
                currentRotationAngle = Mathf.Clamp(currentRotationAngle, -90, 90);
            }
        }

        private void RandomizeRotation()
        {
            if (_switchRotationTimer < 0)
            {
                currentRotationAngle += Random.Range(0, 4) > 1 ? -90f : 90f;
                _switchRotationTimer += Random.Range(angleSwitchTimeRange.x, angleSwitchTimeRange.y);
            }
            else
            {
                _switchRotationTimer -= Time.deltaTime;
            }
        }

        
        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawRay(_ray);
        //     Gizmos.DrawWireSphere(_ray.origin + _ray.direction * detectionDistance, detectionSphereRadius);
        // }
    }
}