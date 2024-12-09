using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boot: MonoBehaviour
{
    [SerializeField] private float force = 50f;
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

            SuperManager.instance.soundManager.PlaySound(SoundType.Collision, 0.5f);
        }
    }
}
