using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private LevelType _levelDestination = LevelType.HUB;

    [SerializeField] private float scaleDownDuration = 1.0f;
    [SerializeField] private AnimationCurve scaleCurve;
    private Vector3 initialScale;
    private float scaleDuration = 5.0f;
    private float alarmscale = -1.0f;

    public bool isPortalActive = true;

    private void Start()
    {

        initialScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            DeactivatePortal();
            LoadLevelFromPortal();
        }
    }
    private void LoadLevelFromPortal()
    {
        SuperManager.instance.uiManager.VisibleToBlack();
        SuperManager.instance.levelManager.LoadLevel(_levelDestination);
    }
    public void DeactivatePortal()
    {
        isPortalActive = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Teleporting, 0.8f, this.transform.position);
        SuperManager.instance.vibrationManager.leftController.TeleportHaptic();
        SuperManager.instance.vibrationManager.rightController.TeleportHaptic();
    }
}
