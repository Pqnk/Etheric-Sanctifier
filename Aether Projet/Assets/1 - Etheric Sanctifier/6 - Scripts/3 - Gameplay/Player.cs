using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _playerHealth = 500.0f;
    [SerializeField] private bool _playerIsDead = false;
    [SerializeField] private float _playerMaxMana = 20.0f;
    [SerializeField] private float _playerCurrentMana = 0.0f;
    [SerializeField] private GameObject _headLight;

    private void Start()
    {
        if (SuperManager.instance.levelManager.GetCurrentLevelType() == LevelType.HUB)
        {
            _headLight.SetActive(false);
        }        
    }

    public void DamagerPlayer(float damage)
    {
        _playerHealth -= damage;

        if(_playerHealth <= 0 )
        {
            _playerIsDead = true;
            SuperManager.instance.gameManagerAetherPunk.PlayerDeadCinematic();
        }
    }

    private void Update()
    {
        if(_playerCurrentMana >= _playerMaxMana)
        {
            _playerCurrentMana = _playerMaxMana;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerCurrentMana = _playerMaxMana;
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
