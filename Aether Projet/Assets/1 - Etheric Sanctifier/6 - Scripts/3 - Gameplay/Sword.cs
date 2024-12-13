using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private float forceImpulse = 200.0f;
    [SerializeField] private int damage = 50;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ghost")
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 collisionPoint = contact.point;

            Vector3 collisionDirection = (collision.transform.position - transform.position).normalized;

            Ghost behaviorGhost = collision.gameObject.GetComponent<Ghost>();
            behaviorGhost.AddForceToGhost(collisionDirection, forceImpulse, ForceMode.Impulse);
            behaviorGhost.LowerHealth(damage);

            PlaySoundAndVFXSword(collisionPoint);
        }
    }

    private void PlaySoundAndVFXSword(Vector3 posContact)
    {
        //SuperManager.instance.vfxManager.InstantiateVFX_vfxSwordImpact(posContact);
        SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Sword, 0.5f, this.transform.position);

        SuperManager.instance.vibrationManager.rightController.BigShootHaptic();
    }
}
