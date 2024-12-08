using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private float force = 1.0f;
    [SerializeField] private float damageMultiplier = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ghost")
        {
            Vector3 collisionDirection = (collision.transform.position - transform.position).normalized;
            Rigidbody rbGhost = collision.gameObject.GetComponent<Rigidbody>();
            rbGhost.AddForce(collisionDirection * force, ForceMode.Impulse);

            float damage = force * damageMultiplier;
            Ghost behaviorGhost = collision.gameObject.GetComponent<Ghost>();
            behaviorGhost.LowerHealth(damage);
        }
    }
}
