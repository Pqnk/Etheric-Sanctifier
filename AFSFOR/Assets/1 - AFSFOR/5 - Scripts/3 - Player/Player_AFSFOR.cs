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

    [Header("Player's Stuff")]
    [SerializeField] private GameObject _headLight;
    [SerializeField] private UIPlayer _UIPlayer;
    #endregion

    #region Awake, Start and Update
    private void Awake()
    {
        _playerCurrentHealth = _playerMaxHealth;
    }
    private void Start()
    {
        if (SuperManager.instance.levelManager.GetCurrentLevelType() == LevelType.HUB)
        {
            _headLight.SetActive(false);
        }

        InitializeUIPlayer();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
        _UIPlayer.SetMaxScore(1000);
    }
    private void UpdateUIPlayer()
    {
        _UIPlayer.SetHealth(_playerCurrentHealth);
        _UIPlayer.SetMana(_playerCurrentMana);
        _UIPlayer.SetScore(_playerCurrentScore);
    }
    #endregion

    #region Mana
    public void AddMana(int mana)
    {
        _playerCurrentMana +=mana;

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

        if (_playerCurrentHealth <= 0)
        {
            _playerIsDead = true;
        }
    }

}
