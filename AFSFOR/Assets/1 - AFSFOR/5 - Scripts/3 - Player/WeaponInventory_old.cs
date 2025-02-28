using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Boolean leftAction = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    [SerializeField] private SteamVR_Action_Boolean rightAction = SteamVR_Input.GetBooleanAction("SnapTurnRight");
    public List<GameObject> weapons;
    private List<GameObject> leftHandWeapons;
    private List<GameObject> rightHandWeapons;
    private int leftIndex = 0, rightIndex = 0;

    private void Start()
    {
        leftHandWeapons = weapons;
        rightHandWeapons = weapons;
        StartingWeapons();
    }

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

                if (leftHandLeftArrow)
                {
                    SwitchWeapon(ref leftIndex, leftHandWeapons, -1);
                }
                if (leftHandRightArrow)
                {
                    SwitchWeapon(ref leftIndex, leftHandWeapons, 1);
                }
            }
            else if (rightHandLeftArrow || rightHandRightArrow)
            {
                SuperManager.instance.timeScaleManager.ToggleSlowMotion(true);

                if (rightHandLeftArrow)
                {
                    SwitchWeapon(ref rightIndex, rightHandWeapons, -1);
                }
                if (rightHandRightArrow)
                {
                    SwitchWeapon(ref rightIndex, rightHandWeapons, 1);
                }
            }
        }
    }

    void SwitchWeapon(ref int index, List<GameObject> weaponsCurrentHand, int direction)
    {
        weaponsCurrentHand[index].SetActive(false);
        index = (index + direction + weaponsCurrentHand.Count) % weaponsCurrentHand.Count;
        if (weaponsCurrentHand[index] == null)
        {
            Debug.Log("No Weapons");
        }
        else
        {
            weaponsCurrentHand[index].SetActive(true);
        }
    }

    void StartingWeapons()
    {
        foreach (var w in leftHandWeapons) w.SetActive(false);
        foreach (var w in rightHandWeapons) w.SetActive(false);
        leftHandWeapons[leftIndex].SetActive(true);
        rightHandWeapons[rightIndex].SetActive(true);
    }
}
