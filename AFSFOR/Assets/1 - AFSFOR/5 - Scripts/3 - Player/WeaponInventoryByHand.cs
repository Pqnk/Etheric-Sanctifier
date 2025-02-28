using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class WeaponInventoryByHand : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Boolean leftAction = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    [SerializeField] private SteamVR_Action_Boolean rightAction = SteamVR_Input.GetBooleanAction("SnapTurnRight");
    public List<GameObject> weapons;
    private int currentIndex = 0;
    public bool isLeftHand = false;

    [SerializeField] private float _switchTimer = 0.5f;
    private bool _readyToSwitch = true;

    [SerializeField] private Hand hand;

   void Start() 
    { 
        StartingWeapons(); 
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

        if ( (handLeftArrow || handRightArrow) && _readyToSwitch)
        {
            Debug.Log("Switch ! ");
            _readyToSwitch = false;
            StartCoroutine(SelectionCountDown());
            SuperManager.instance.timeScaleManager.ToggleSlowMotion(true);

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

    void SwitchWeapon(int direction)
    {
        weapons[currentIndex].SetActive(false);
        currentIndex = (currentIndex + direction + weapons.Count) % weapons.Count;
        weapons[currentIndex].SetActive(true);

        if(currentIndex == 0)
        {
            hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithoutController);
        }
        else
        {
            hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithController);

        }
    }

    void StartingWeapons()
    {
        foreach (var w in weapons) w.SetActive(false);
        weapons[currentIndex].SetActive(true);
    }

    IEnumerator SelectionCountDown()
    {
        yield return new WaitForSecondsRealtime(_switchTimer);
        _readyToSwitch = true;
    }

}
