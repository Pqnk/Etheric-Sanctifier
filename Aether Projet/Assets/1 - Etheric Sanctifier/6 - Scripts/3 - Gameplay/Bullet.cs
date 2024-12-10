using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float forcePush;
    public float bulletSpeed;

    private void Start()
    {
        //StartCoroutine(DestroyBullet());
    }

    private void Update()
    {
        //transform.Translate(transform.forward * (bulletSpeed * Time.deltaTime), Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ghost")
        {
            GameObject ghost = other.transform.parent.gameObject;
            Ghost scriptGhost = ghost.GetComponent<Ghost>();
            Rigidbody rbGhost = ghost.transform.parent.GetComponent<Rigidbody>();

            Vector3 collisionDirection = (ghost.transform.position - transform.position).normalized;
            rbGhost.AddForce(collisionDirection * forcePush, ForceMode.Impulse);

            scriptGhost.LowerHealth(damage);

            SuperManager.instance.soundManager.PlaySound(SoundType.Collision, 0.5f);

            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Object")
        {
            Destroy(gameObject);
        }
    }
    

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}
