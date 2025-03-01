using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System;

public class WeaponInventoryByHand : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Boolean leftAction = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    [SerializeField] private SteamVR_Action_Boolean rightAction = SteamVR_Input.GetBooleanAction("SnapTurnRight");

    [SerializeField] private UIIventory _UiIventory;

    [SerializeField] private List<Weapon> _weapons;
    private int currentIndex = 0;
    public bool isLeftHand = false;

    [SerializeField] private float _switchTimer = 0.5f;
    private bool _readyToSwitch = true;

    [SerializeField] private Hand hand;

    void Start()
    {
        StartingWeapons();

        //_UiIventory.InitializeUIIventory(_weapons);
    }

    void Update()
    {
        bool handLeftArrow = false;
        bool handRightArrow = false;

        if (isLeftHand)
        {
            handLeftArrow = leftAction.GetStateDown(SteamVR_Input_Sources.LeftHand);
            handRightArrow = rightAction.GetStateDown(SteamVR_Input_Sources.LeftHand);
        }
        else
        {
            handLeftArrow = leftAction.GetStateDown(SteamVR_Input_Sources.RightHand);
            handRightArrow = rightAction.GetStateDown(SteamVR_Input_Sources.RightHand);
        }

        if ((handLeftArrow || handRightArrow) && _readyToSwitch)
        {
            _readyToSwitch = false;
            StartCoroutine(SelectionCountDown());

            OnSlowMoStarted();

            if (handLeftArrow)
            {
                SwitchWeapon(-1);
            }
            if (handRightArrow)
            {
                SwitchWeapon(1);
            }
        }
    }

    private void OnSlowMoStarted()
    {
        SuperManager.instance.timeScaleManager.ToggleSlowMotion(true);
        TimeScaleManager.OnSlowMoFinished += OnSlowMoDone;
        _weapons[currentIndex].ToggleActivationWeapons(false);
    }
    private void OnSlowMoDone()
    {
        _weapons[currentIndex].ToggleActivationWeapons(true);
        TimeScaleManager.OnSlowMoFinished -= OnSlowMoDone;
    }

    void SwitchWeapon(int direction)
    {
        _weapons[currentIndex].transform.gameObject.SetActive(false);
        _weapons[currentIndex].isWeaponEquiped = false;

        currentIndex = (currentIndex + direction + _weapons.Count) % _weapons.Count;

        _weapons[currentIndex].transform.gameObject.SetActive(true);
        _weapons[currentIndex].isWeaponEquiped = true;

        if (currentIndex == 0)
        {
            hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithoutController);
        }
        else
        {
            hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithController);

        }

        CallUpdateUIIventory();
    }

    void StartingWeapons()
    {
        foreach (var w in _weapons) w.transform.gameObject.SetActive(false);
        _weapons[currentIndex].transform.gameObject.SetActive(true);
        CallUpdateUIIventory();
    }

    IEnumerator SelectionCountDown()
    {
        yield return new WaitForSecondsRealtime(_switchTimer);
        _readyToSwitch = true;
    }


    public void CallUpdateUIIventory()
    {
        Sprite centerImageTmp, leftImageTmp, rightImageTmp;

        int leftIndex   = (currentIndex - 1 + _weapons.Count) % _weapons.Count;
        int rightIndex  = (currentIndex + 1) % _weapons.Count;

        centerImageTmp  = _weapons[currentIndex].weaponIcon;
        leftImageTmp   = _weapons[leftIndex].weaponIcon;
        rightImageTmp   = _weapons[rightIndex].weaponIcon;

        _UiIventory.SetUIImages(centerImageTmp, leftImageTmp, rightImageTmp);
    }
}
