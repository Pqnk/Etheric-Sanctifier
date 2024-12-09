using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float forcePush;
    public float bulletSpeed;

    private void Update()
    {
        transform.Translate(transform.forward * (bulletSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ghost")
        {
            GameObject ghost = collision.gameObject;
            Ghost scriptGhost = ghost.GetComponent<Ghost>();

            Vector3 collisionDirection = (ghost.transform.position - transform.position).normalized;
            Rigidbody rbGhost = ghost.gameObject.GetComponent<Rigidbody>();
            rbGhost.AddForce(collisionDirection * forcePush, ForceMode.Impulse);

            scriptGhost.LowerHealth(damage);

            SuperManager.instance.soundManager.PlaySound(SoundType.Collision, 0.5f);
        }
    }
}
