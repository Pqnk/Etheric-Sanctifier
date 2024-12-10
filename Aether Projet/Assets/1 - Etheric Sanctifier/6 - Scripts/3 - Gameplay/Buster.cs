using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Presets;
using UnityEngine;

public class Buster : MonoBehaviour
{
    [SerializeField] int indexPalier;
    [SerializeField] float[] outerAngleLight;
    [SerializeField] float[] innerAngleLight;
    [SerializeField] float[] intensityLight;

    private void Start()
    {
        indexPalier = SuperManager.instance.gameManagerAetherPunk.Get_Palier();
        ApplyChangeBuster(indexPalier);
    }

    /*  EXPLICATION DES FONCTION UTILIE
    SuperManager.instance.gameManagerAetherPunk.Get_KillGhost() -> récupere le nombre de ghost tué
    SuperManager.instance.gameManagerAetherPunk.palierKills[i] -> récupere le nombre de ghost a tué par palier
    SuperManager.instance.gameManagerAetherPunk.Get_Palier() -> récupere le palier actuel
    SuperManager.instance.gameManagerAetherPunk.Set_NextPalier() -> passe au palier suplementaire
    SuperManager.instance.gameManagerAetherPunk.Set_KillGhost(false) -> remet le nombre de ghost tué a 0 
    */

    private void Update()
    {
        int kills = SuperManager.instance.gameManagerAetherPunk.Get_KillGhost();

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            SuperManager.instance.gameManagerAetherPunk.Set_KillGhost(false);
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            SuperManager.instance.gameManagerAetherPunk.Set_KillGhost(true);
        }

        if (kills >= SuperManager.instance.gameManagerAetherPunk.palierKills[indexPalier])
        {
            SuperManager.instance.gameManagerAetherPunk.Set_ResetGhost();

            if (indexPalier < SuperManager.instance.gameManagerAetherPunk.palierKills.Count)
            {
                SuperManager.instance.gameManagerAetherPunk.Set_NextPalier(true);
                indexPalier = SuperManager.instance.gameManagerAetherPunk.Get_Palier();
                ApplyChangeBuster(indexPalier);
            }
        }
        else if (kills < 0)
        {
            SuperManager.instance.gameManagerAetherPunk.Set_ResetGhost();

            if (indexPalier > 0)
            {
                indexPalier--;
                ApplyChangeBuster(indexPalier);
                SuperManager.instance.gameManagerAetherPunk.Set_NextPalier(false);
            }
        }
    }

    private void ApplyChangeBuster(int indexPalier)
    {
        Light light = transform.GetChild(1).gameObject.GetComponent<Light>();
        MeshRenderer mesh = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();

        light.innerSpotAngle = intensityLight[indexPalier];
        light.intensity = intensityLight[indexPalier];
        light.spotAngle = outerAngleLight[indexPalier];
    }
}
