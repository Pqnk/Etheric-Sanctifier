using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSounds : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private KeyCode triggerKeyCollision = KeyCode.X;
    [SerializeField] private KeyCode triggerKeyAmbiance = KeyCode.Space;

    void Update()
    {
        // Collision sound test
        if(Input.GetKeyDown(triggerKeyCollision)) 
        {
            SuperManager.instance.soundManager.PlaySound(SoundType.Collision, 0.5f);
        }

        // Ambiance Music Test
        if (Input.GetKeyDown(triggerKeyAmbiance))
        {
            SuperManager.instance.soundManager.PlaySound(SoundType.Music, 0.2f);
        }
    }
}
