using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class MeteorController : MonoBehaviour
    {
        [SerializeField] private float fallSpeed;
        
        private Rigidbody _rb;

        [SerializeField, ReadOnly] private bool falling;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        [Button]
        public void SpawnMeteor(float speed)
        {
            fallSpeed = speed;
            falling = true;
            _rb.AddForce(transform.forward * fallSpeed, ForceMode.VelocityChange);
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
        }
    }
}
