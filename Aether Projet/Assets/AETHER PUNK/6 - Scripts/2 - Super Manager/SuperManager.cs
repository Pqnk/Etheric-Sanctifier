using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperManager : MonoBehaviour
{
    public static SuperManager instance { get; private set; }

   
    public GameManagerAetherPunk gameManagerAetherPunk { get; private set; }
    public TutoManager tutoManager { get; private set; }
    public SoundManager soundManager { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        gameManagerAetherPunk = GetComponent<GameManagerAetherPunk>();
        tutoManager = GetComponent<TutoManager>();
        soundManager = GetComponent<SoundManager>();
    }
}
