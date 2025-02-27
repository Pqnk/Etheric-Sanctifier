using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Boolean leftAction = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    [SerializeField] private SteamVR_Action_Boolean rightAction = SteamVR_Input.GetBooleanAction("SnapTurnRight");

    public List<Weapon> weapons;

    private void Update()
    {
        if (leftAction != null && rightAction != null && leftAction.activeBinding && rightAction.activeBinding)
        {
            Debug.Log("Action !");

        }
    }

}
