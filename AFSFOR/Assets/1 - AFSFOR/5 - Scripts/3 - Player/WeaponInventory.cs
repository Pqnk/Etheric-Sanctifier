using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Boolean leftAction = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    [SerializeField] private SteamVR_Action_Boolean rightAction = SteamVR_Input.GetBooleanAction("SnapTurnRight");
    public List<GameObject> weapons;

    private void Update()
    {
        if (leftAction != null && rightAction != null && leftAction.activeBinding && rightAction.activeBinding)
        {
            bool leftHandLeftArrow = leftAction.GetStateDown(SteamVR_Input_Sources.LeftHand);
            bool leftHandRightArrow = rightAction.GetStateDown(SteamVR_Input_Sources.LeftHand);
            bool rightHandLeftArrow = leftAction.GetStateDown(SteamVR_Input_Sources.RightHand);
            bool rightHandRightArrow = rightAction.GetStateDown(SteamVR_Input_Sources.RightHand);

            if (leftHandLeftArrow || leftHandRightArrow)
            {
                SuperManager.instance.timeScaleManager.ToggleSlowMotion(true);
            }
            else if (rightHandLeftArrow || rightHandRightArrow)
            {
                SuperManager.instance.timeScaleManager.ToggleSlowMotion(true);
            }
        }
    }
}
