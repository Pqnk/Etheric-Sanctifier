using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float forcePush;
    public float bulletSpeed;
    public float rangeHeavyImpact;
    public bool isHeavyShoot;
    public LayerMask LayerMask;

    private void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    private void Update()
    {
        transform.Translate(transform.forward * (bulletSpeed * Time.deltaTime), Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isHeavyShoot)
        {
            if (other.gameObject.tag == "Ghost")
            {
                GameObject ghost = other.gameObject;
                Ghost scriptGhost = ghost.GetComponent<Ghost>();
                Rigidbody rbGhost = ghost.GetComponent<Rigidbody>();


                GameObject vfxLight = SuperManager.instance.vfxManager.InstantiateVFX_vfxLightImpact(this.transform);

                Vector3 collisionDirection = (ghost.transform.position - transform.position).normalized;
                rbGhost.AddForce(collisionDirection * forcePush, ForceMode.Impulse);

                scriptGhost.LowerHealth(damage);

                SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.ShootImpact, 0.5f, this.transform.position);


                Destroy(transform.root.gameObject);
            }
            else if (other.gameObject.tag == "Object")
            {
                GameObject vfxLight = SuperManager.instance.vfxManager.InstantiateVFX_vfxLightImpact(this.transform);
                SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.ShootImpact, 0.5f, this.transform.position);
                Destroy(transform.root.gameObject);
            }
        }
        else
        {
            if (other.gameObject.tag == "Ghost")
            {
                GameObject ghost = other.gameObject;
                Ghost scriptGhost = ghost.GetComponent<Ghost>();
                Rigidbody rbGhost = ghost.GetComponent<Rigidbody>();


                GameObject vfxHeavy = SuperManager.instance.vfxManager.InstantiateVFX_vfxHeavyImpact(this.transform);

                Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, rangeHeavyImpact, LayerMask);

                foreach (var collider in hitColliders)
                {
                    GameObject ghostCollider = collider.gameObject;

                    if (((1 << collider.gameObject.layer) & LayerMask) != 0)
                    {
                        ghostCollider.GetComponent<Ghost>().LowerHealth(damage);
                    }
                }

                SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.ShootBigImpact, 0.5f, this.transform.position);
                Destroy(transform.root.gameObject);
            }
            else if (other.gameObject.tag == "Object")
            {
                GameObject vfxHeavy = SuperManager.instance.vfxManager.InstantiateVFX_vfxHeavyImpact(this.transform);
                SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.ShootBigImpact, 0.5f, this.transform.position);
                Destroy(transform.root.gameObject);
            }
        }

        //if (other.gameObject.tag == "Ghost")
        //{
        //    GameObject ghost = other.gameObject;
        //    Ghost scriptGhost = ghost.GetComponent<Ghost>();
        //    Rigidbody rbGhost = ghost.GetComponent<Rigidbody>();

        //    if (!isHeavyShoot)
        //    {
        //        GameObject vfxLight = SuperManager.instance.vfxManager.InstantiateVFX_vfxLightImpact(this.transform);

        //        Vector3 collisionDirection = (ghost.transform.position - transform.position).normalized;
        //        rbGhost.AddForce(collisionDirection * forcePush, ForceMode.Impulse);

        //        scriptGhost.LowerHealth(damage);

        //        SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Collision, 0.5f, this.transform.position);
        //    }
        //    else
        //    {
        //        GameObject vfxHeavy = SuperManager.instance.vfxManager.InstantiateVFX_vfxHeavyImpact(this.transform);

        //        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, rangeHeavyImpact, LayerMask);

        //        foreach (var collider in hitColliders)
        //        {
        //            GameObject ghostCollider = collider.gameObject;

        //            if (((1 << collider.gameObject.layer) & LayerMask) != 0)
        //            {
        //                ghostCollider.GetComponent<Ghost>().LowerHealth(damage);
        //            }
        //        }

        //        SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Collision, 0.5f, this.transform.position);
        //    }

        //    Destroy(transform.root.gameObject);
        //}
        //else if (other.gameObject.tag == "Object")
        //{
        //    Destroy(transform.root.gameObject);
        //}
    }


    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(3f);

        Destroy(transform.root.gameObject);
    }
}
