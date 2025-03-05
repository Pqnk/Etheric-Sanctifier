using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIPlayer : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _manaBar;
    [SerializeField] private Slider _scoreBar;
    [SerializeField] private GameObject _damageUI;
    [SerializeField] private GameObject _diedText;
    [SerializeField] private float _damageUICoolDown = 1.0f;
    private Coroutine _coroutineDamageUI = null;

    private void Start()
    {
        _damageUI.SetActive(false);
        _diedText.SetActive(false);
    }

    #region Health
    public void SetMaxHealth(int maxHealth)
    {
        _healthBar.maxValue = maxHealth;
        _healthBar.value = maxHealth;
    }
    public void SetHealth(int health)
    {
        _healthBar.value = health;
    }
    #endregion

    #region Score
    public void SetMaxScore(int maxScore)
    {
        _scoreBar.maxValue = maxScore;
        _scoreBar.value = 0;
    }

    public void SetScore(int score)
    {
        _scoreBar.value = score;
    }
    #endregion

    #region UI Damage
    public void StartUIDamage()
    {
        if (_coroutineDamageUI == null)
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
    #endregion

    #region Mana
    public void SetMaxMana(int maxMana)
    {
        _manaBar.maxValue = maxMana;
        _manaBar.value = 0;
    }
    public void SetMana(int mana)
    {
        _manaBar.value = mana;
    }

    #endregion

    public void ToggleDeadText(bool activate)
    {
        _diedText.SetActive(activate);
    }

    public void ToggleUIDamage(bool activate)
    {
        _damageUI.SetActive(activate);
    }
}
