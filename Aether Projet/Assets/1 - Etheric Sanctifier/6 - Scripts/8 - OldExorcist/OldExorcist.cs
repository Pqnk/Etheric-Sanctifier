using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldExorcist : MonoBehaviour
{
    [SerializeField] private float _timeBeforeVoice = 3.0f;
    [SerializeField] private GameObject _portals;
    [SerializeField] private AudioSource _npcSource;
    private Portal _portalTutoRef;
    private Portal _portalMissionRef;

    private bool _alreadySpeaking = false;

    private void Awake()
    {
        _portals.SetActive(false);
        _npcSource = GetComponent<AudioSource>();

        _portalTutoRef = _portals.transform.GetChild(1).GetComponent<Portal>();
        _portalMissionRef = _portals.transform.GetChild(2).GetComponent<Portal>();
    }

    void Start()
    {
        StartCoroutine(StartIntroVoice());
        SuperManager.instance.soundManager.PlaySound(SoundType.HUB, 0.05f);
    }

    private void Update()
    {
        if (!_portalTutoRef.isPortalActive && !_alreadySpeaking)
        {
            _portalMissionRef.DeactivatePortal();
            _alreadySpeaking = true;
            _npcSource.Stop();
            _npcSource.clip = SuperManager.instance.voiceManager.GetVoice(VoiceType.ChoiceTuto);
            _npcSource.Play();
        }

        if (!_portalMissionRef.isPortalActive && !_alreadySpeaking)
        {
            _portalTutoRef?.DeactivatePortal();
            _alreadySpeaking = true;
            _npcSource.Stop();
            _npcSource.clip = SuperManager.instance.voiceManager.GetVoice(VoiceType.ChoiceMission);
            _npcSource.Play();
        }
    }

    IEnumerator StartIntroVoice()
    {
        yield return new WaitForSeconds(_timeBeforeVoice);
        _npcSource.clip = SuperManager.instance.voiceManager.GetVoice(VoiceType.Intro);
        _npcSource.Play();
        yield return new WaitForSeconds(20.0f);
        _portals.SetActive(true);
    }
}
