using System;
using UnityEngine;

namespace Game
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject spawnerPrefab;

        private void Awake()
        {
            SpawnObject();
        }

        public virtual GameObject SpawnObject()
        {
            return Instantiate(spawnerPrefab, transform.position, Quaternion.identity);
        }
    }
}
