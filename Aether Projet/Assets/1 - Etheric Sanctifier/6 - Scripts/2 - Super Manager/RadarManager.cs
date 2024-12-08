using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RadarManager : MonoBehaviour
{
    [SerializeField] private GameObject radar;

    public void ToggleRadar(bool newActivation)
    {
        radar.SetActive(newActivation);
    }

}
