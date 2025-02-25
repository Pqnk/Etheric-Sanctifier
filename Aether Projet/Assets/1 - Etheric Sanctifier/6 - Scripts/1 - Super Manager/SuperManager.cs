using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperManager : MonoBehaviour
{
    public static SuperManager instance { get; private set; }

    public GameManagerAFSFOR gameManagerAetherPunk { get; private set; }
    public SoundManager soundManager { get; private set; }
    public VoiceManager voiceManager { get; private set; }
    public VFXManager vfxManager { get; private set; }
    public GhostManager ghostManager { get; private set; }
    public UIManager uiManager { get; private set; }
    public LevelManager levelManager { get; private set; }
    public VibrationManager vibrationManager { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        gameManagerAetherPunk = GetComponent<GameManagerAFSFOR>();
        soundManager = GetComponent<SoundManager>();
        voiceManager = GetComponent<VoiceManager>();
        vfxManager = GetComponent<VFXManager>();
        ghostManager = GetComponent<GhostManager>();
        uiManager = GetComponent<UIManager>();
        levelManager = GetComponent<LevelManager>();
        vibrationManager = GetComponent<VibrationManager>();
    }
}
