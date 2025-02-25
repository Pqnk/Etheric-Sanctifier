using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 50;
    public float forcePush;
    public float bulletSpeed;
    public float rangeHeavyImpact;
    public bool isHeavyShoot;
    public LayerMask LayerMask;
    public float lifeTimeBullet = 5.0f;

    private void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    private void Update()
    {
        Movebullet();
    }

    //  ###########################################
    //  ############  BULLET MOVEMENT  ############
    //  ###########################################
    private void Movebullet()
    {
        transform.Translate(transform.forward * (bulletSpeed * Time.deltaTime), Space.World);
    }

    //  ###########################################
    //  ##########  BULLET TRIGGER ENTER  #########
    //  ###########################################
    private void OnTriggerEnter(Collider other)
    {
        if (!isHeavyShoot)
        {
            if (other.gameObject.tag == "Ghost")
            {
                Ghost scriptGhost = other.gameObject.GetComponent<Ghost>();
                scriptGhost.AddForceToGhostOppositeToTarget(forcePush, ForceMode.Impulse);
                scriptGhost.LowerHealth(damage);

                PlaySoundAndFXShootImpact();
                Destroy(transform.root.gameObject);
            }
            else if (other.gameObject.tag == "Object")
            {
                PlaySoundAndFXShootImpact();
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
                Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, rangeHeavyImpact, LayerMask);

                foreach (var collider in hitColliders)
                {
                    GameObject ghostCollider = collider.gameObject;

                    if (((1 << collider.gameObject.layer) & LayerMask) != 0)
                    {
                        ghostCollider.GetComponent<Ghost>().LowerHealth(damage);
                    }
                }

                PlaySoundAndFXShootBigImpact();
                Destroy(transform.root.gameObject);
            }
            else if (other.gameObject.tag == "Object")
            {
                PlaySoundAndFXShootBigImpact();
                Destroy(transform.root.gameObject);
            }
        }
    }


    //  ###########################################
    //  ##########  BULLET SOUND AND FX  ##########
    //  ###########################################
    private void PlaySoundAndFXShootBigImpact()
    {
        SuperManager.instance.vfxManager.InstantiateVFX_vfxHeavyImpact(this.transform);
        SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.ShootBigImpact, 0.5f, this.transform.position);
    }
    private void PlaySoundAndFXShootImpact()
    {
        SuperManager.instance.vfxManager.InstantiateVFX_vfxLightImpact(this.transform);
        SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.ShootImpact, 0.5f, this.transform.position);
    }

    //  ###########################################
    //  ############  BULLET DESTROY  #############
    //  ###########################################
    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(lifeTimeBullet);

        Destroy(transform.root.gameObject);
    }
}
