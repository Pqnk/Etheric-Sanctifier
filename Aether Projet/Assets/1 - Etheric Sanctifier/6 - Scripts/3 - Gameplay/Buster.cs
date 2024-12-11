using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MaterialList
{
    public List<Material> materials = new List<Material>();
}

public class Buster : MonoBehaviour
{
    [SerializeField] int indexPalier;

    [Header("Gestion de la light")]
    [SerializeField] float[] outerAngleLight;
    [SerializeField] float[] innerAngleLight;
    [SerializeField] float[] intensityLight;

    [Header("Image du timer")]
    [SerializeField] Image sliderImage;

    [Header("Liste de groupes de matériaux")]
    [SerializeField] List<MaterialList> materialGroups = new List<MaterialList>();

    private float currentTimer;
    private float maxTime;
    private bool nextWave = false;

    private void Start()
    {
        indexPalier = SuperManager.instance.gameManagerAetherPunk.Get_Palier();
        maxTime = SuperManager.instance.ghostManager.timeBetweenWave;
        ApplyChangeBuster(indexPalier);
    }

    private void Update()
    {
        CheckPalier();
        TimerBuster();

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            SuperManager.instance.gameManagerAetherPunk.Set_KillGhost(false);
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            SuperManager.instance.gameManagerAetherPunk.Set_KillGhost(true);
        }
    }

    private void TimerBuster()
    {
        if (nextWave)
        {
            currentTimer += Time.deltaTime;

            sliderImage.fillAmount = currentTimer / maxTime;

            if (currentTimer > maxTime)
            {
                nextWave = false;
                currentTimer = 0;
                sliderImage.fillAmount = 0;
            }
        }
    }

    private void CheckPalier()
    {
        int kills = SuperManager.instance.gameManagerAetherPunk.Get_KillGhost();

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
        nextWave = true;

        Light light = transform.GetChild(1).gameObject.GetComponent<Light>();
        MeshRenderer mesh = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();

        switch (indexPalier)
        {
            case 1:
                SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.ShootBigImpact, 0.5f, this.transform.position);
                GameObject vfxLight1 = SuperManager.instance.vfxManager.InstantiateVFX_VFXPalierValidated(this.transform);
                SuperManager.instance.voiceManager.PlayVoiceAtLocation(VoiceType.FirstPalier, 0.5f, this.transform);
                break;

            case 2:
                SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.ShootBigImpact, 0.5f, this.transform.position);
                GameObject vfxLight2 = SuperManager.instance.vfxManager.InstantiateVFX_VFXPalierValidated(this.transform);
                SuperManager.instance.voiceManager.PlayVoiceAtLocation(VoiceType.SecondPalier, 0.5f, this.transform);
                break;

            case 3:
                SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.ShootBigImpact, 0.5f, this.transform.position);
                GameObject vfxLight3 = SuperManager.instance.vfxManager.InstantiateVFX_VFXPalierValidated(this.transform);
                SuperManager.instance.voiceManager.PlayVoiceAtLocation(VoiceType.Victory, 0.5f, this.transform);
                break;
        }

        light.innerSpotAngle = intensityLight[indexPalier];
        light.intensity = intensityLight[indexPalier];
        light.spotAngle = outerAngleLight[indexPalier];

        MaterialList groupMat = materialGroups[indexPalier];
        mesh.SetMaterials(groupMat.materials);
    }
}
