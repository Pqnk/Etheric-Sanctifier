using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AFSFOR : MonoBehaviour
{
    [SerializeField] private int _playerMaxHealth = 500;
    [SerializeField] private int _playerCurrentHealth = 0;
    [SerializeField] private bool _playerIsDead = false;
    [SerializeField] private float _playerMaxMana = 20.0f;
    [SerializeField] private float _playerCurrentMana = 0.0f;
    [SerializeField] private int _playerCurrentScore = 0;
    [SerializeField] private GameObject _headLight;
    [SerializeField] private UIPlayer _UIPlayer;

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
        if (_playerCurrentMana >= _playerMaxMana)
        {
            _playerCurrentMana = _playerMaxMana;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerCurrentMana = _playerMaxMana;
        }

        UpdateUIPlayer();

    }

    private void InitializeUIPlayer()
    {
        _UIPlayer.SetMaxHealth(_playerMaxHealth);
        _UIPlayer.SetMaxScore(1000);
    }

    private void UpdateUIPlayer()
    {
        _UIPlayer.SetHealth(_playerCurrentHealth);
        _UIPlayer.SetScore(_playerCurrentScore);
    }

    public void DamagerPlayer(float damage)
    {
        _playerCurrentHealth -= (int)Mathf.Round(damage);

        if (_playerCurrentHealth <= 0)
        {
            _playerIsDead = true;
            SuperManager.instance.gameManagerAFSFOR.PlayerDeadCinematic();
        }
    }


    public void AddMana()
    {
        _playerCurrentMana++;
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
}
