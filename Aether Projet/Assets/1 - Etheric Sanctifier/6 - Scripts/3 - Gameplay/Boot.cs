using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boot: MonoBehaviour
{
    [SerializeField] private float force = 150f;
    [SerializeField] private float damage = 25.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ghost")
        {
            GameObject vfxHeavy = SuperManager.instance.vfxManager.InstantiateVFX_vfxFootImpact(this.transform);

            Ghost behaviorGhost = collision.gameObject.GetComponent<Ghost>();
            behaviorGhost.LowerHealth(damage);
            behaviorGhost.AddForceToGhostOppositeToTarget(force, ForceMode.Impulse);

            SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Sword, 0.2f, this.transform.position);
        }
    }
}
