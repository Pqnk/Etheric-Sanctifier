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
        this.transform.GetChild(2).gameObject.SetActive(false);
        Vector3 initialScalelle = transform.localScale;
        float elapsedTime = 0f;
        float startValue = 1f;
        float endValue = 0f;
        while (elapsedTime < scaleDuration)
        {
            float t = elapsedTime / scaleDuration;
            float currentValue = Mathf.Lerp(startValue, endValue, t);
            transform.localScale = initialScalelle * currentValue;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.zero;
    }
}
