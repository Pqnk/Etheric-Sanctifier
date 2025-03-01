using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIIventory : MonoBehaviour
{
    [SerializeField] private Image leftWeaponImage;
    [SerializeField] private Image centerWeaponImage;
    [SerializeField] private Image rightWeaponImage;

    public void SetUIImages(Sprite centerImage, Sprite leftImage, Sprite rightImage)
    {
        centerWeaponImage.sprite = centerImage;
        leftWeaponImage.sprite = leftImage;
        rightWeaponImage.sprite = rightImage;
    }
}
