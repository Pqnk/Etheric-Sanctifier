using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _playerHealth = 500.0f;
    [SerializeField] private bool _playerIsDead = false;

    public void DamagerPlayer(float damage)
    {
        _playerHealth -= damage;

        if(_playerHealth <= 0 )
        {
            _playerIsDead = true;
            SuperManager.instance.gameManagerAetherPunk.PlayerDeadCinematic();
        }
    }

}
