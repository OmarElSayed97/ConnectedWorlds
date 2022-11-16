using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Misc
{
    public class AutoAlign : MonoBehaviour
    {

        [SerializeField] private bool alignOnAwakeOnly = true;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float planetDetectionRadius = 5f;
        
        private Collider[] _colliders;
        [SerializeField, ReadOnly] private Transform planet;

        public Transform PlanetTransform => planet;
        
        private void Awake()
        {
            _colliders = new Collider[2];
            if (alignOnAwakeOnly)
                FixOrientation();
        }

        private void OnEnable()
        {
            if (!alignOnAwakeOnly)
                FixOrientation();
        }

        private void DetectPlanet()
        {
            var trans = transform;
            var size = Physics.OverlapSphereNonAlloc(trans.position, planetDetectionRadius, _colliders, groundMask);
            
            var minSquareDistance = Mathf.Infinity;

            for (var i = 0; i < size; i++)
            {
                var currentDistance = (_colliders[i].transform.position - trans.position).sqrMagnitude;
                if (!(currentDistance < minSquareDistance)) continue;
                
                minSquareDistance = currentDistance;
                planet = _colliders[i].transform;
            }
        }

        [Button]
        public void FixOrientation()
        {
            if (_colliders.IsNullOrEmpty())
                _colliders = new Collider[2];
            
            if (!planet)
                DetectPlanet();

            if (!planet) return;
            
            var currentTransform = transform;
            var planetTransform = planet.transform;

            var deltaPosition = currentTransform.position - planetTransform.position;
            var normalizedDeltaPosition = deltaPosition.normalized;

            var ray = new Ray(currentTransform.position + normalizedDeltaPosition * planetDetectionRadius, -normalizedDeltaPosition);

            Debug.DrawRay(ray.origin, ray.direction * (2 * planetDetectionRadius), Color.red, 0.5f);
            if (Physics.Raycast(ray, out var hit, planetDetectionRadius * 2, groundMask))
            {
                transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(Vector3.ProjectOnPlane(currentTransform.forward, hit.normal), hit.normal));
            }
            else
            {
                planet = null;
            }

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, planetDetectionRadius);
        }
    }
}
