using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIPlayer : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _scoreBar;
    [SerializeField] private GameObject _damageUI;
    [SerializeField] private float _damageUICoolDown = 1.0f;
    private Coroutine _coroutineDamageUI;

    private void Start()
    {
        _damageUI.SetActive(false);
    }

    public void SetMaxHealth(int maxHealth)
    {
        _healthBar.maxValue = maxHealth;
        _healthBar.value = maxHealth;
    }

    public void SetHealth(int health)
    {
        _healthBar.value = health;
    }

    public void SetMaxScore(int maxScore)
    {
        _scoreBar.maxValue = maxScore;
        _scoreBar.value = 0;
    }

    public void SetScore(int score)
    {
        _scoreBar.value = score;
    }

    public void StartUIDamage()
    {
        if (_coroutineDamageUI != null)
        {
            _coroutineDamageUI = StartCoroutine(UIDamageCoolDown());
        }
        else
        {
            StopCoroutine(_coroutineDamageUI);
           _coroutineDamageUI = StartCoroutine(UIDamageCoolDown());
        }
    }
    IEnumerator UIDamageCoolDown()
    {
        _damageUI.SetActive(true);
        yield return new WaitForSecondsRealtime(_damageUICoolDown);
        _coroutineDamageUI = null;
        _damageUI.SetActive(false);

    }
}
