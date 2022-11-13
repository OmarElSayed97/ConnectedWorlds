using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Misc
{
    public class CircleSpawner : MonoBehaviour
    {
        [SerializeField, OnValueChanged(nameof(UpdateChildren))] private float radius;
        [SerializeField] private Transform parent;
        [SerializeField, ReadOnly] private Transform[] transforms; 


        private void UpdateChildren()
        {
            var angle = Mathf.PI * 2 / transforms.Length;
            
            for (var index = 0; index < transforms.Length; index++)
            {
                var child = transforms[index];
                child.localPosition = new Vector3(radius * Mathf.Cos(angle * index), 0, radius * Mathf.Sin(angle * index));
            }
        }

        [Button]
        private void SetRefs()
        {
            transforms = new Transform[parent.childCount];
            for (var i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                transforms[i] = child;
            }
            UpdateChildren();
        }
    }
}
