using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldExorcistTuto : MonoBehaviour
{
    [SerializeField] private float _timeBeforeVoice = 3.0f;
    [SerializeField] private GameObject _portal;
    [SerializeField] private AudioSource _npcSource;
    private Portal _portalBackToHubRef;

    private bool _alreadySpeaking = false;

    private void Awake()
    {
        _npcSource = GetComponent<AudioSource>();
        _portalBackToHubRef = _portal.transform.GetComponent<Portal>();
    }

    void Start()
    {
        StartCoroutine(StartIntroTutoVoice());
    }

    private void Update()
    {
        if (!_portalBackToHubRef.isPortalActive && !_alreadySpeaking)
        {
            _alreadySpeaking = true;
            _npcSource.Stop();
            _npcSource.clip = SuperManager.instance.voiceManager.GetVoice(VoiceType.ChoiceTuto);
            _npcSource.Play();
        }
    }

    IEnumerator StartIntroTutoVoice()
    {
        yield return new WaitForSeconds(_timeBeforeVoice);
        _npcSource.clip = SuperManager.instance.voiceManager.GetVoice(VoiceType.Tuto01);
        _npcSource.Play();
    }
}
