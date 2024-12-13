using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using Valve.VR;

public class ViveControllerVibration : MonoBehaviour
{
    [SerializeField] private SteamVR_Input_Sources hand;
    [SerializeField] private SteamVR_Action_Vibration hapticAction;

    private void TriggerHapticPulse(float duration, float frequency, float amplitude)
    {
        if (hapticAction != null)
        {
            hapticAction.Execute(0, duration, frequency, amplitude, hand);
        }
    }


    //  #####################################
    //  ###  Methods Called For Haptics  ####
    //  #####################################
    public void ShootHaptic()
    {
        TriggerHapticPulse(0.3f, 55.0f, 0.7f);
    }
    public void BigShootHaptic()
    {
        TriggerHapticPulse(1.5f, 75.0f, 1.0f);
    }


    public IEnumerator StartHapticFeedback()
    {
        while (true)
        {
            TriggerHapticPulse(Time.deltaTime, 40.0f, 1.0f);
            yield return null;
        }
    }


}
