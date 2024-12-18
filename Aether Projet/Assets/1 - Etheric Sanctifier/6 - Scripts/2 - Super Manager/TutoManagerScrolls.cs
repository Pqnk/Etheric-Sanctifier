using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoManagerScrolls : MonoBehaviour
{
    [Header("Scrolls container")]
    [SerializeField] private GameObject _scrollsTutoParent;

    [Header("Tutorial caracteristics")]
    [SerializeField] private float _timeToLaunchTuto = 3.0f;

    [Header("Array of all scrolls")]
    [SerializeField] private GameObject[] _scrollsTuto;

    private Scroll currentScroll;
    private bool _isAlreadyNext = false;
    private int _currentIndexScroll = 0;

    [Header("To test !")]
    public bool istest = false;

    private void Start()
    {
        if (!istest)
        {
            _scrollsTuto = GetAllScrolls();
            currentScroll = _scrollsTuto[_currentIndexScroll].GetComponent<Scroll>();
            StartCoroutine(LaunchTutorial());
        }
        else
        {
            FinishTutorial();
        }
    }
    private void Update()
    {
        if (!istest)
        {
            if (currentScroll.isLearned && !_isAlreadyNext)
            {
                StartCoroutine(NextScrollTutorial());
            }
        }
    }

    public GameObject[] GetAllScrolls()
    {
        int childCount = _scrollsTutoParent.transform.childCount;
        GameObject[] children = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            children[i] = _scrollsTutoParent.transform.GetChild(i).gameObject;
            children[i].SetActive(false);
        }

        return children;
    }

    IEnumerator LaunchTutorial()
    {
        yield return new WaitForSeconds(_timeToLaunchTuto);
        _scrollsTuto[_currentIndexScroll].SetActive(true);
    }
    public void FinishTutorial()
    {
        SuperManager.instance.ghostManager.SetCanSpawn(true);
        SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.SearchingObjective, 0.5f, this.transform.position);
        SuperManager.instance.radarManager.ToggleRadar(true);
    }
    IEnumerator NextScrollTutorial()
    {
        _isAlreadyNext = true;
        _scrollsTuto[_currentIndexScroll].SetActive(false);
        SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Collision, 0.5f, this.transform.position);

        yield return new WaitForSeconds(2);

        _currentIndexScroll++;
        if (_currentIndexScroll < _scrollsTuto.Length)
        {
            currentScroll = _scrollsTuto[_currentIndexScroll].GetComponent<Scroll>();
            _scrollsTuto[_currentIndexScroll].SetActive(true);
            _isAlreadyNext = false;
        }
        else
        {
            FinishTutorial();
        }
    }
}
