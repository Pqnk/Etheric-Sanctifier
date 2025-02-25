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
    [SerializeField] GameObject[] lights;

    [Header("Image du timer")]
    [SerializeField] Image sliderImage;

    [Header("Liste de groupes de matériaux")]
    [SerializeField] List<MaterialList> materialGroups = new List<MaterialList>();

    [SerializeField] GameManagerAFSFOR gm;

    private float currentTimer;
    private float maxTime;
    private bool nextWave = false;

    private void Start()
    {
        gm = SuperManager.instance.gameManagerAFSFOR;
        indexPalier = SuperManager.instance.gameManagerAFSFOR.Get_Palier();
        maxTime = SuperManager.instance.ghostManager.timeBetweenWave;
        ApplyChangeBuster(indexPalier);

        StartCoroutine(PlayBriefingVoice());
    }

    private void Update()
    {
        CheckPalier();
        TimerBuster();

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            SuperManager.instance.gameManagerAFSFOR.Set_KillGhost(false);
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            SuperManager.instance.gameManagerAFSFOR.Set_KillGhost(true);
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
        if (SuperManager.instance.gameManagerAFSFOR.Get_Palier() < 3)
        {
            int kills = SuperManager.instance.gameManagerAFSFOR.Get_KillGhost();

            if (kills >= SuperManager.instance.gameManagerAFSFOR.palierKills[indexPalier])
            {
                SuperManager.instance.gameManagerAFSFOR.Set_ResetGhost();

                if (indexPalier < SuperManager.instance.gameManagerAFSFOR.palierKills.Count)
                {
                    SuperManager.instance.gameManagerAFSFOR.Set_NextPalier(true);
                    indexPalier = SuperManager.instance.gameManagerAFSFOR.Get_Palier();
                    ApplyChangeBuster(indexPalier);
                }
            }
            else if (kills < 0)
            {
                SuperManager.instance.gameManagerAFSFOR.Set_ResetGhost();

                if (indexPalier > 0)
                {
                    indexPalier--;
                    ApplyChangeBuster(indexPalier);
                    SuperManager.instance.gameManagerAFSFOR.Set_NextPalier(false);
                }
            }
        }
    }

    private void ApplyChangeBuster(int indexPalier)
    {
        nextWave = true;

        MeshRenderer mesh = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();

        foreach (var item in lights)
        {
            item.gameObject.SetActive(false);
        }

        lights[indexPalier].SetActive(true);

        switch (indexPalier)
        {
            case 1:
                SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.ShootBigImpact, 0.5f, this.transform.position);
                GameObject vfxLight1 = SuperManager.instance.vfxManager.InstantiateVFX_VFXPalierValidated(this.transform);
                SuperManager.instance.voiceManager.PlayVoiceAtLocation(VoiceType.FirstPalier, 1f, this.transform);
                break;

            case 2:
                SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.ShootBigImpact, 0.5f, this.transform.position);
                GameObject vfxLight2 = SuperManager.instance.vfxManager.InstantiateVFX_VFXPalierValidated(this.transform);
                SuperManager.instance.voiceManager.PlayVoiceAtLocation(VoiceType.SecondPalier, 0.5f, this.transform);
                break;

            case 3:
                SuperManager.instance.ghostManager.DefinitiveStopWaveAndClearGhosts();
                SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.ShootBigImpact, 0.5f, this.transform.position);
                GameObject vfxLight3 = SuperManager.instance.vfxManager.InstantiateVFX_VFXPalierValidated(this.transform);
                SuperManager.instance.voiceManager.PlayVoiceAtLocation(VoiceType.Victory, 0.5f, this.transform);
                SuperManager.instance.levelManager.BackToHub();
                break;
        }


        MaterialList groupMat = materialGroups[indexPalier];
        mesh.SetMaterials(groupMat.materials);
    }

    IEnumerator PlayBriefingVoice()
    {
        yield return new WaitForSeconds(3.5f);
        SuperManager.instance.voiceManager.PlayVoiceAtLocation(VoiceType.BriefingMission, 0.5f, SuperManager.instance.ghostManager.GetMainTarget());
    }
}
