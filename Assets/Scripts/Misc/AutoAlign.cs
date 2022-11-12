using System;
using UnityEngine;

namespace Misc
{
    public class AutoAlign : MonoBehaviour
    {

        [SerializeField] private bool alignOnAwakeOnly = true;
        [SerializeField] private LayerMask groundMask;
        
        private Collider[] _colliders;
        private Transform _planet;
        
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
            var size = Physics.OverlapSphereNonAlloc(trans.position, 5f, _colliders, groundMask);
            
            var minSquareDistance = Mathf.Infinity;

            for (var i = 0; i < size; i++)
            {
                var currentDistance = (_colliders[i].transform.position - trans.position).sqrMagnitude;
                if (!(currentDistance < minSquareDistance)) continue;
                
                minSquareDistance = currentDistance;
                _planet = _colliders[i].transform;
            }
        }

        private void FixOrientation()
        {
            DetectPlanet();

            if (!_planet) return;
            
            var currentTransform = transform;
            var planetTransform = _planet.transform;

            var deltaPosition = currentTransform.position - planetTransform.position;
            var normalizedDeltaPosition = deltaPosition.normalized;

            var ray = new Ray(currentTransform.position + normalizedDeltaPosition * 5f, -normalizedDeltaPosition);

            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 0.5f);
            if (Physics.Raycast(ray, out var hit, 10f, groundMask))
            {
                transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(Vector3.ProjectOnPlane(currentTransform.forward, hit.normal), hit.normal));
            }

        }

    }
}
