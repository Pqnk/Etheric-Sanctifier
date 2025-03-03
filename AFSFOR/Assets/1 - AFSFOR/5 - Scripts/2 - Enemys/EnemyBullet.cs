using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 50;
    public float bulletSpeed;
    public float rangeHeavyImpact;
    public float lifeTimeBullet = 5.0f;
    [Space]
    public bool isBossBullet;

    private bool _isDeflected = false;
    [SerializeField] private GameObject _meshSkullGoat;

    private Vector3 _oppositeDirection;

    public Transform targetEnemyBullet;

    private void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    private void Update()
    {
        Movebullet();
    }

    private void Movebullet()
    {
        if (!_isDeflected)
        {
            transform.Translate(transform.forward * (bulletSpeed * Time.deltaTime), Space.World);
        }
        else
        {
            transform.Translate(-_oppositeDirection * (bulletSpeed*5 * Time.deltaTime), Space.World);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player_AFSFOR playerScript = other.gameObject.GetComponent<Player_AFSFOR>();
            playerScript.DamagerPlayer(damage);

            // Si gros tir alors empecher le joueur de tirer pendant x Secondes
            if (isBossBullet)
            {

            }

            PlaySoundAndFX();
        }
        else if (other.gameObject.tag == "Object")
        {
            PlaySoundAndFX();
        }
        else if (other.gameObject.tag == "Sword" && !_isDeflected)
        {
            _meshSkullGoat.transform.Rotate(0, 180, 0, Space.Self);
            _oppositeDirection = (targetEnemyBullet.position - transform.position).normalized;
            _isDeflected = true;
        }
    }

    private void PlaySoundAndFX()
    {
        if (isBossBullet)
        {
            SuperManager.instance.vfxManager.InstantiateVFX_vfxHeavyImpact(this.transform);
            SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.ShootBigImpact, 0.5f, this.transform.position);
        }
        else
        {
            SuperManager.instance.vfxManager.InstantiateVFX_vfxLightImpact(this.transform);
            SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.ShootImpact, 0.5f, this.transform.position);
        }

        Destroy(gameObject);
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(lifeTimeBullet);
        Destroy(gameObject);
    }

}
