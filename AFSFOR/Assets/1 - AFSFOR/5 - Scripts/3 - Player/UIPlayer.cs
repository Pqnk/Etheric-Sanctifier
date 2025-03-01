using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIPlayer : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _scoreBar;

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
}
