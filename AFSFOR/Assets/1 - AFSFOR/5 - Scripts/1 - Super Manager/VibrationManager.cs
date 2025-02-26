using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VibrationManager : MonoBehaviour
{
    private LevelManager levelManager;

    [Header("Controllers References")]
    public ViveControllerVibration rightController;
    public ViveControllerVibration leftController;

    void Start()
    {
        levelManager = SuperManager.instance.levelManager;
        InitializeVibrationManager();
    }
    public void InitializeVibrationManager()
    {
        GameObject g = levelManager.FindInScene(levelManager.GetCurrentLevelType(), "Camera_Player");
                
        GameObject cam = GameObject.Find("Camera_Player");

        //leftController = g.transform.GetChild(0).gameObject.GetComponent<ViveControllerVibration>();
        //rightController = g.transform.GetChild(1).gameObject.GetComponent<ViveControllerVibration>();

        leftController = cam.transform.GetChild(0).gameObject.GetComponent<ViveControllerVibration>();
        rightController = cam.transform.GetChild(1).gameObject.GetComponent<ViveControllerVibration>();
    }
}
