using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class MeteorSpawner : MonoBehaviour
    {
        [SerializeField] private MeteorController meteorPrefab;

        [SerializeField] private Transform planet;
        [SerializeField] private Transform container;
        [SerializeField] private float planetRadius;

        [SerializeField, MinMaxSlider(0f, 60f, true)]
        private Vector2 spawnIntervalRange;
        
        [SerializeField, MinMaxSlider(0f, 1000f, true)]
        private Vector2 spawnRadiusRange;
        
        [SerializeField, MinMaxSlider(5f, 20f, true)]
        private Vector2 spawnSpeedRange;

        private float _spawnTimer;
        
        private MeteorController _dummyMeteorController;
        
        
        private void SpawnMeteor()
        {
            _dummyMeteorController = Instantiate(meteorPrefab, planet.position, Random.rotation, container);
            Transform meteorTransform;
            (meteorTransform = _dummyMeteorController.transform).Translate(Vector3.back * Random.Range(spawnRadiusRange.x, spawnRadiusRange.y));
            meteorTransform.Translate(Random.insideUnitCircle * planetRadius);
            _dummyMeteorController.SpawnMeteor(Random.Range(spawnSpeedRange.x, spawnRadiusRange.y));
        }

        private void HandleSpawning()
        {
            if (_spawnTimer <= 0)
            {
                SpawnMeteor();
                _spawnTimer = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
            }
            else
            {
                _spawnTimer -= Time.deltaTime;
            }
        }
        
        private void Update()
        {
            HandleSpawning();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.gray;
            var position = planet.position;
            
            Gizmos.DrawWireSphere(position, planetRadius);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, spawnRadiusRange.x);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, spawnRadiusRange.y);
        }
    }
}
