using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroBuster : MonoBehaviour
{
    [Header("Parametre Light")]
    [SerializeField] Light busterLight;
    [SerializeField] float currentRangeLight;
    float[] valeurLightPalier = { 4, 10, 20, 30 };

    private void Start()
    {
        currentRangeLight = valeurLightPalier[0];
    }

    private void Update()
    {
        busterLight.range = currentRangeLight;
    }
}
