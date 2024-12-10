using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldExorcist : MonoBehaviour
{
    void Start()
    {
        SuperManager.instance.voiceManager.PlayVoice(VoiceType.Intro, 0.5f, this.transform);
    }

    void Update()
    {
        
    }
}
