using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private LevelType _levelDestination = LevelType.HUB;

    [SerializeField] private float scaleDownDuration = 1.0f;
    [SerializeField] private AnimationCurve scaleCurve;
    private Vector3 initialScale;
    private float duration = 5.0f;
    private float alarmscale = -1.0f;

    public bool isPortalActive = true;

    private void Start()
    {

        initialScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
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
        StartCoroutine(ScaleDownToZero());
    }

    private IEnumerator ScaleDownToZero()
    {
        alarmscale = Time.time + duration;
        while (alarmscale > Time.time )
        {
            float progress = Time.time / alarmscale;
            float scaleMultiplier = scaleCurve != null ? scaleCurve.Evaluate(progress) : 1 - progress;

            transform.localScale = initialScale * scaleMultiplier;
            yield return null;
        }
        transform.localScale = Vector3.zero;
    }
}
