using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Prefab")]
    [SerializeField] private GameObject _uiPrefab;
    private GameObject _ui;

    [Header("Black Screen")]
    [SerializeField] private Image _blackScreen;
    [SerializeField] private float _fadeDuration = 3.0f;
    private bool _isFading = false;

    private void Start()
    {
        InstantiateUI();
    }

    public void InstantiateUI()
    {
        _ui = Instantiate(_uiPrefab);
        _blackScreen = _ui.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>();

        DontDestroyOnLoad(_ui);

        StartCoroutine(FadeToTransparent());

    }
    public IEnumerator FadeToTransparent()
    {
        _isFading = true;

        float elapsedTime = 0f;
        Color initialColor;

        if (_blackScreen != null)
        {
            initialColor = _blackScreen.color;
        }
        else
        {
            Debug.LogWarning("No Image or SpriteRenderer assigned!");
            yield break;
        }

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / _fadeDuration);

            if (_blackScreen != null)
            {
                _blackScreen.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            }

            yield return null;
        }

        if (_blackScreen != null)
        {
            _blackScreen.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        }

        _isFading = false;
    }
    public IEnumerator FadeToOpaque()
    {
        _isFading = true;

        float elapsedTime = 0f;
        Color initialColor;

        if (_blackScreen != null)
        {
            initialColor = _blackScreen.color;
        }
        else
        {
            Debug.LogWarning("No Image or SpriteRenderer assigned!");
            yield break;
        }

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / _fadeDuration);

            if (_blackScreen != null)
            {
                _blackScreen.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            }

            yield return null;
        }

        if (_blackScreen != null)
        {
            _blackScreen.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);
        }

        _isFading = false;
    }
}
