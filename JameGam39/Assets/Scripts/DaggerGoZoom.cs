using System;
using DG.Tweening;
using UnityEngine;

public class DaggerGoZoom : MonoBehaviour
{
    [SerializeField] private float homingStrength;
    private Transform _target;
    private Rigidbody2D _rb;

    
    private void Awake()
    {
        Invoke(nameof(GoGo), 1.5f);
        _rb = GetComponent<Rigidbody2D>();
        _target = FindObjectOfType<EnemyController>()?.transform;
        Destroy(gameObject, 2.5f);
    }

    private void GoGo()
    {
        if(_target != null)
        {
            var distance = _target.position - transform.position;
            var forceMagnitude = homingStrength / (distance.magnitude * distance.magnitude);
            _rb.AddForce(distance * forceMagnitude);
            //transform.rotation = Quaternion.LookRotation(Vector3.forward * 500, _rb.velocity);
            // Calculate the forward direction from velocity
            Vector3 forwardDirection = _rb.velocity.normalized;

            // Ensure we have a non-zero velocity before setting the rotation
            if (forwardDirection != Vector3.zero)
            {
                // Create a rotation looking in the direction of the velocity with an upward vector of Vector3.up
                Quaternion targetRotation = Quaternion.LookRotation(forwardDirection, Vector3.up);
                // Adjust rotation by adding 90 degrees on the z-axis
                targetRotation *= Quaternion.Euler(0, 0, 90);

                // Apply the rotation to the transform
                transform.rotation = targetRotation;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().Damage(2f);
            Destroy(gameObject);
        }
    }
}
