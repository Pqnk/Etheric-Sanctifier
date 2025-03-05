using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damageLight = 5;
    public int damageHeavy = 100000;
    public float forcePush;
    public float bulletSpeed;
    public float rangeHeavyImpact;
    public bool isHeavyShoot;
    public LayerMask LayerMask;
    public float lifeTimeBullet = 5.0f;

    public GameObject hitcanvas;

    private void Start()
    {
        damageLight = SuperManager.instance.damageManager.damagePlayerGUNLIGHT;
        damageHeavy = SuperManager.instance.damageManager.damagePlayerGUNHEAVY;

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
                Vector3 contactPoint = other.ClosestPoint(transform.position);
                SuperManager.instance.damageManager.SpawnHitCanvas(damageLight, transform.position);

                Enemy scriptEnemy = other.gameObject.GetComponent<Enemy>();
                scriptEnemy.AddForceBack(forcePush, ForceMode.Impulse);
                scriptEnemy.Get_Hit(damageLight);

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
                SuperManager.instance.damageManager.SpawnHitCanvas(damageHeavy, transform.position);

                Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, rangeHeavyImpact, LayerMask);

                foreach (var collider in hitColliders)
                {
                    GameObject ghostCollider = collider.gameObject;

                    if (((1 << collider.gameObject.layer) & LayerMask) != 0)
                    {
                        ghostCollider.GetComponent<Enemy>().Get_Hit(damageHeavy);
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
