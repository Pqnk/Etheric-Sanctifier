using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _ui;

    public void InstantiateUI()
    {
        Instantiate(_ui);
    }
}
