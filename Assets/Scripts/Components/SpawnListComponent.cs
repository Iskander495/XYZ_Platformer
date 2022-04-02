using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Components
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField] private SpawnData[] _spawners;

        public void Spawn(string id)
        {
            _spawners.FirstOrDefault(x => x.Id.Equals(id))?
                .Component
                .Process();
        }

        [Serializable]
        public class SpawnData
        {
            public string Id;

            public SpawnComponent Component;
        }
    }
}