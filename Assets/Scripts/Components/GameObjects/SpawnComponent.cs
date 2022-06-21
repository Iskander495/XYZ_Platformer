using UnityEngine;
using Utils;

namespace Components.GameObjects
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var newObj = SpawnUtils.Spawn(_prefab, _target.position);
            newObj.transform.localScale = _target.lossyScale;
            newObj.SetActive(true);
        }
    }
}