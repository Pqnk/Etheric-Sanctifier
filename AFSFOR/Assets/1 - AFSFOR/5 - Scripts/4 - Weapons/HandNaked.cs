using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandNaked : Weapon
{
    [SerializeField] private float handForce = 100.0f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ghost")
        {
            other.gameObject.GetComponent<Enemy>().StartExpulsion();
            SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Slap, 0.5f, this.gameObject.transform.position);
        }
    }

}
