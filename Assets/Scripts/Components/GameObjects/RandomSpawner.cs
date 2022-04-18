using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Components.GameObjects
{
    public class RandomSpawner : MonoBehaviour
    {
        [SerializeField] private float _sectorAngle = 60;
        [SerializeField] private float _sectorRotation = 1;

        [SerializeField] private float _waitTime = 0.1f;
        [SerializeField] private float _speed = 6;

        private Coroutine _routine;

        public void StartDrop(GameObject[] items)
        {
            TryStopRoutine();

            _routine = StartCoroutine(StartSpawn(items));
        }

        private void TryStopRoutine()
        {
            if (_routine != null)
                StopCoroutine(_routine);
        }

        private IEnumerator StartSpawn(GameObject[] items)
        {
            for (var i = 0; i < items.Length; i++)
            {
                Spawn(items[i]);

                yield return new WaitForSeconds(_waitTime);
            }
        }

        private void Spawn(GameObject item)
        {
            var instance = Instantiate(item, transform.position, Quaternion.identity);
            var rigidBody = instance.GetComponent<Rigidbody2D>();

            var randomAngle = Random.Range(0, _sectorAngle);
            var forceVector = AngleToVectorInSector(randomAngle);

            rigidBody.AddForce(forceVector * _speed, ForceMode2D.Impulse);
        }

        private Vector2 AngleToVectorInSector(float angle)
        {
            var angleMiddleDelta = (180 - _sectorRotation - _sectorAngle) / 2;
            return GetUnitOnCircle(angle + angleMiddleDelta);
        }

        private Vector3 GetUnitOnCircle(float angleDegrees)
        {
            var angleRadians = angleDegrees * Mathf.PI / 180.0f;

            var x = Mathf.Cos(angleRadians);
            var y = Mathf.Sin(angleRadians);

            return new Vector3(x, y, 0);
        }

        private void OnDrawGizmosSelected()
        {
            var position = transform.position;

            var middleAngleDelta = (180 - _sectorRotation - _sectorAngle) / 2;
            var rightBound = GetUnitOnCircle(middleAngleDelta);
            Handles.DrawLine(position, position + rightBound);

            var leftBound = GetUnitOnCircle(middleAngleDelta + _sectorAngle);
            Handles.DrawLine(position, position + leftBound);
            Handles.DrawWireArc(position, Vector3.forward, rightBound, _sectorAngle, _sectorRotation);

            Handles.color = new Color(1f, 1f, 1f, 0.1f);
            Handles.DrawSolidArc(position, Vector3.forward, rightBound, _sectorAngle, _sectorRotation);


        }
    }
}