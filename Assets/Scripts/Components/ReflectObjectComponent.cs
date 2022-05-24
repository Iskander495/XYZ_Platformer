using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectObjectComponent : MonoBehaviour
{
    [SerializeField] private float _degrees;

    public void ReflectOnTrigger(GameObject go)
    {
        var rb = go.GetComponent<Rigidbody2D>();
        Vector3 velocities = rb.velocity;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;

        var rad = _degrees * Mathf.Deg2Rad;

        var x1 = velocities.x * Mathf.Cos(rad) + velocities.y * Mathf.Sin(rad);
        var y1 = velocities.y * Mathf.Cos(rad) - velocities.x * Mathf.Sin(rad);
        var nv = new Vector2(x1, y1);
        //var nv = Vector3.Reflect(transform.position.normalized, velocities);

        rb.AddForce(nv, ForceMode2D.Impulse);
    }

    public void ReflectOnCollision(GameObject go)
    {
        var rb = go.GetComponent<Rigidbody2D>();

        Vector2 inDirection = GetComponent<Rigidbody2D>().velocity;
        Vector2 inNormal = transform.position.normalized;
        Vector2 newVelocity = Vector2.Reflect(inDirection, inNormal);

        rb.AddForce(newVelocity, ForceMode2D.Impulse);
    }
}
