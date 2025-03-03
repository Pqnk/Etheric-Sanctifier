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

    private GameObject _playerRef;

    private void Start()
    {
        StartCoroutine(DestroyBullet());

        _playerRef = GameObject.Find("--Player_AFSFOR--");
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
            _meshSkullGoat.transform.Rotate(0, 180, 0, Space.Self);
            Vector3 direction = (_playerRef.transform.position - transform.position).normalized;
            transform.Translate(-direction * (bulletSpeed*5 * Time.deltaTime), Space.World);
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
        else if (other.gameObject.tag == "Sword")
        {
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
