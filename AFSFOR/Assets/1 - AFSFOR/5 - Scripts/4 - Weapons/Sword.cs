using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    [SerializeField] private float forceImpulse = 200.0f;
    [SerializeField] private int damage = 50;

    private void Start()
    {
        damage = SuperManager.instance.damageManager.damagePlayerSWORD;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (areWeaponsActive)
        {
            if (collision.gameObject.tag == "Ghost")
            {
                ContactPoint contact = collision.contacts[0];
                Vector3 collisionPoint = contact.point;
                collision.gameObject.GetComponent<Enemy>().Get_Hit(damage);
                collision.gameObject.GetComponent<Enemy>().StartExpulsion(10000);

                SuperManager.instance.damageManager.SpawnHitCanvas(damage, collisionPoint);

                PlaySoundAndVFXSword(collisionPoint);
            }
        }
    }

    private void PlaySoundAndVFXSword(Vector3 posContact)
    {
        SuperManager.instance.vfxManager.InstantiateVFX_vfxSwordImpact(posContact);
        SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Sword, 0.5f, this.transform.position);

        SuperManager.instance.vibrationManager.rightController.BigShootHaptic();
    }
}
