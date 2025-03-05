using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AFSFOR : MonoBehaviour
{
    #region Attributes
    [Header("Player's Death Boolean")]
    [SerializeField] private bool _playerIsDead = false;

    [Header("Player's Health")]
    [SerializeField] private int _playerMaxHealth = 100;
    [SerializeField] private int _playerCurrentHealth = 0;

    [Header("Player's Mana")]
    [SerializeField] private int _playerMaxMana = 20;
    [SerializeField] private int _playerCurrentMana = 0;

    [Header("Player's Score")]
    [SerializeField] private int _playerCurrentScore = 0;
    [SerializeField] private int _playerMaxScore = 100;

    [Header("Player's Stuff")]
    [SerializeField] private GameObject _headLight;
    [SerializeField] private UIPlayer _UIPlayer;

    public GameObject VRHead;

    private StatManager _damageManager;
    #endregion

    #region Start and Update
    private void Start()
    {
        InitializePlayerFromDamageManager();

        if (SuperManager.instance.levelManager.GetCurrentLevelType() == LevelType.HUB)
        {
            ToggleHeadLight(false);
        }

        InitializeUIPlayer();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            _playerCurrentMana = _playerMaxMana;
        }

        UpdateUIPlayer();
    }
    #endregion

    #region Player's UI
    private void InitializeUIPlayer()
    {
        _UIPlayer.SetMaxHealth(_playerMaxHealth);
        _UIPlayer.SetHealth(_playerCurrentHealth);
        _UIPlayer.SetMaxMana(_playerMaxMana);
        _UIPlayer.SetMana(_playerCurrentMana);
        _UIPlayer.SetMaxScore(_playerMaxScore);
    }
    private void UpdateUIPlayer()
    {
        _UIPlayer.SetHealth(_playerCurrentHealth);
        _UIPlayer.SetMana(_playerCurrentMana);

        if(SuperManager.instance.levelManager.GetCurrentLevelType() == LevelType.Level01)
        {
            _playerCurrentScore = GameManager.instance.enemiesKilledThisWave;
            _UIPlayer.SetScore(_playerCurrentScore);
            _playerMaxScore = GameManager.instance.enemiesToKillThisWave;
            _UIPlayer.SetMaxScore(_playerMaxScore);
        }
    }
    #endregion

    #region Mana
    public void AddMana(int mana)
    {
        _playerCurrentMana += mana;

        if (_playerCurrentMana >= _playerMaxMana)
        {
            _playerCurrentMana = _playerMaxMana;
        }

    }
    public void AsShootRail()
    {
        _playerCurrentMana = 0;
    }
    public float Get_playerCurrentMana()
    {
        return _playerCurrentMana;
    }
    public float Get_playerMaxMana()
    {
        return _playerMaxMana;
    }
    #endregion

    public void DamagerPlayer(float damage)
    {
        _UIPlayer.StartUIDamage();
        _playerCurrentHealth -= (int)Mathf.Round(damage);
        SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Ouch, 0.5f, this.transform.position);

        if (_playerCurrentHealth <= 0 && !_playerIsDead)
        {
            Weapon.areWeaponsActive = false;
            _playerIsDead = true;
            _UIPlayer.ToggleDeadText(true);
            _UIPlayer.ToggleUIDamage(true);
            SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Death, 0.5f, this.transform.position);
            StartCoroutine(LoadingHUb());
        }
    }

    public void RefillPlayerLife()
    {
        _playerCurrentHealth = _playerMaxHealth;
        _UIPlayer.SetHealth(_playerCurrentHealth);
    }

    private void InitializePlayerFromDamageManager()
    {
        _damageManager = SuperManager.instance.damageManager;
        _playerMaxHealth = _damageManager.lifePlayer;
        _playerCurrentHealth = _playerMaxHealth;
    }

    public void ToggleHeadLight(bool activate)
    {
        _headLight.SetActive(activate);
    }
    


    IEnumerator LoadingHUb()
    {
        yield return new WaitForSecondsRealtime(5);
        _UIPlayer.ToggleDeadText(false);
        _UIPlayer.ToggleUIDamage(false);
        SuperManager.instance.levelManager.LoadLevel(LevelType.HUB);
    }
}
