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
        damage = SuperManager.instance.damageManager.damagePlayerGUN;

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
                Enemy scriptEnemy = other.gameObject.GetComponent<Enemy>();
                scriptEnemy.AddForceBack(forcePush, ForceMode.Impulse);
                scriptEnemy.Get_Hit(damage);

                PlaySoundAndFXShootImpact();
                Destroy(transform.root.gameObject);
            }
            else if (other.gameObject.tag == "Object")
            {
                PlaySoundAndFXShootImpact();
                Destroy(transform.root.gameObject);
            }
            else if(other.gameObject.tag == "EnemyBullet")
            {
                PlaySoundAndFXShootImpact();
                Destroy(other.gameObject);
            }
        }
        else
        {
            if (other.gameObject.tag == "Ghost")
            {
                Vector3 contactPoint = other.ClosestPoint(transform.position);
                SuperManager.instance.damageManager.SpawnHitCanvas(damage, contactPoint);

                Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, rangeHeavyImpact, LayerMask);

                foreach (var collider in hitColliders)
                {
                    GameObject ghostCollider = collider.gameObject;

                    if (((1 << collider.gameObject.layer) & LayerMask) != 0)
                    {
                        //ghostCollider.GetComponent<Ghost>().LowerHealth(damage);
                        ghostCollider.GetComponent<Enemy>().Get_Hit(damage);
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
