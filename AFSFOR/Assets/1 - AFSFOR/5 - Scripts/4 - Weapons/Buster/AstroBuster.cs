using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroBuster : MonoBehaviour
{
    [Header("Parametre Light")]
    [SerializeField] Light busterLight;
    [SerializeField] float currentRangeLight;

    float[] valeurLightPalier = { 4, 10, 20, 30 };
    int points = 0;
    int currentPoints = 0;

    private void Start()
    {
        currentRangeLight = valeurLightPalier[0];
    }

    private void Update()
    {
        busterLight.range = currentRangeLight;
    }

    public void Add_Points()
    {

    }
}
