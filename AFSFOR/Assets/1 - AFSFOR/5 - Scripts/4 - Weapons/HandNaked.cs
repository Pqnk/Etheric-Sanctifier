using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandNaked : Weapon
{
    [SerializeField] private float handForce = 100.0f;
    [SerializeField] private int handDamage = 10;

    private void Start()
    {
        handDamage = SuperManager.instance.damageManager.damagePlayerHAND;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ghost")
        {
            other.gameObject.GetComponent<Enemy>().StartExpulsion();
            SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Slap, 0.5f, this.gameObject.transform.position);
        }
    }

}
